using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace BaseModule
{
    public class ProcedureUpdateVersion : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            UpdateVersion(procedureOwner).Forget();
        }

        private async UniTaskVoid UpdateVersion(ProcedureOwner procedureOwner)
        {
            await UniTask.Yield();

            try
            {
                var package = GameModule.Resource.GetPackage();
                var updatePackageVersionOperation = package.UpdatePackageVersionAsync();
                await updatePackageVersionOperation.Task;

                if (updatePackageVersionOperation.Status != EOperationStatus.Succeed)
                {
                    Debug.LogError($"ResUpdateVersionState:获取远程package version文件失败!");
                    // 如果获取远端资源清单失败，说明当前网络无连接。
                    // 在正常开始游戏之前，需要验证本地清单内容的完整性[注意只需要保证gameplay标签的资源，按需下载的不用]。
                    string packageVersion = package.GetPackageVersion();
                    var operation = package.PreDownloadContentAsync(packageVersion);
                    await operation.Task;
                    if (operation.Status != EOperationStatus.Succeed)
                    {
                        Debug.LogError($"ResUpdateVersionState:验证本地资源不完整!");
                        // 资源内容本地并不完整，需要提示玩家联网更新。
                        LauncherNoticePanel.Show(new LauncherNoticePanel.Param()
                        {
                            IsShowCloseBtn = false,
                            IsShowCancelBtn = false,
                            DescStr = LauncherLocalizationManager.Instance().GetText("UI_Desc_Download_Error"),
                            ConfirmCallBack = () => { OnUpdateVersionFailed(procedureOwner); }
                        });
                        return;
                    }

                    var downloader = operation.CreateResourceDownloader("BASE_RES",
                        GameModule.Resource.DownloadingMaxNum, GameModule.Resource.FailedTryAgain);
                    if (downloader.TotalDownloadCount > 0)
                    {
                        // 资源内容本地并不完整，需要提示玩家联网更新。
                        LauncherNoticePanel.Show(new LauncherNoticePanel.Param()
                        {
                            IsShowCloseBtn = false,
                            IsShowCancelBtn = false,
                            DescStr = LauncherLocalizationManager.Instance().GetText("UI_Desc_Download_Error"),
                            ConfirmCallBack = () => { OnUpdateVersionFailed(procedureOwner); }
                        });
                        return;
                    }

                    Debug.Log($"YooAsset UpdatePackageVersionAsync Fail, But Local Res Is Ready!");

                    ChangeState<ProcedureLoadAssembly>(procedureOwner);
                }
                else
                {
                    GameModule.Resource.PackageVersion = updatePackageVersionOperation.PackageVersion;
                    
                    Debug.Log($"YooAsset Updated package Version : to {updatePackageVersionOperation.PackageVersion}");

                    ChangeState<ProcedureUpdateManifest>(procedureOwner);
                }
            }
            catch (Exception e)
            {
                OnUpdateVersionFailed(procedureOwner);
            }
        }

        private void OnUpdateVersionFailed(ProcedureOwner procedureOwner)
        {
            // 资源内容本地并不完整，需要提示玩家联网更新。
            LauncherNoticePanel.Show(new LauncherNoticePanel.Param()
            {
                IsShowCloseBtn = false,
                IsShowCancelBtn = false,
                DescStr = LauncherLocalizationManager.Instance().GetText("UI_Desc_Download_Error"),
                ConfirmCallBack = () => { UpdateVersion(procedureOwner).Forget(); }
            });
        }
    }
}