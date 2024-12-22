using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace BaseModule
{
    public class ProcedureUpdateManifest : ProcedureBase
    {
        public override bool UseNativeDialog => true;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Debug.Log("更新资源清单！！！");
            
            UpdateManifest(procedureOwner).Forget();
        }
        
        private async UniTaskVoid UpdateManifest(ProcedureOwner procedureOwner)
        {
            await UniTask.Yield();
            
            var manifestOperation = GameModule.Resource.UpdatePackageManifestAsync(GameModule.Resource.PackageVersion);
            await manifestOperation.Task;
            
            if (manifestOperation.Status != EOperationStatus.Succeed)
            {
                Debug.LogError($"YooAsset Package : UpdatePackageManifestAsync Error!");
                
                LauncherNoticePanel.Show(new LauncherNoticePanel.Param()
                {
                    IsShowCloseBtn = false,
                    IsShowCancelBtn = false,
                    DescStr = LauncherLocalizationManager.Instance().GetText("UI_Desc_Update_Net_Error"),
                    ConfirmCallBack = () =>
                    {
                        UpdateManifest(procedureOwner).Forget();
                    }
                });
            }
            else
            {
                ChangeState<ProcedureDownload>(procedureOwner);
            }
        }
    }
}