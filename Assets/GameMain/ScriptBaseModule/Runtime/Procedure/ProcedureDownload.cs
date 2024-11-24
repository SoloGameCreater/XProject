using Cysharp.Threading.Tasks;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace BaseModule
{
    public class ProcedureDownload : ProcedureBase
    {
        public override bool UseNativeDialog => true;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Download(procedureOwner).Forget();
        }
        
        private async UniTaskVoid Download(ProcedureOwner procedureOwner)
        {
            await UniTask.Yield();
            
            var package = GameModule.Resource.GetPackage();
            var downloader = package.CreateResourceDownloader("BASE_RES", GameModule.Resource.DownloadingMaxNum, GameModule.Resource.FailedTryAgain, GameModule.Resource.Timeout);
            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log($"YooAsset Not found any download files !");
                
                ChangeState<ProcedureLoadAssembly>(procedureOwner);
            }
            else
            {
                LoadingPanel.Instance.SetProgress(0.0f);
                LoadingPanel.Instance.SetProgressTextFormat("");
                
                //当开始下载某个文件
                downloader.OnStartDownloadFileCallback += (name, bytes) =>
                {
                    Debug.Log($"YooAsset Start Download file : {name}, bytes: {bytes}");
                };
                //当某个文件下载失败
                downloader.OnDownloadErrorCallback += (name, error) =>
                {
                    Debug.LogWarning($"YooAsset Download file : {name} Error! Reason : {error}");
                };
                //当下载进度发生变化
                downloader.OnDownloadProgressCallback += (int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes) =>
                {
                    float progress = currentDownloadBytes * 1.0f / totalDownloadBytes;
                    if (currentDownloadBytes == totalDownloadBytes)
                    {
                        progress = 1.0f;
                    }

                    var downloadStr = FormatBytes(currentDownloadBytes);
                    var totalStr    = FormatBytes(totalDownloadBytes);
                    var formatStr   = $"{downloadStr}/{totalStr}";
                    LoadingPanel.Instance.SetProgress(progress);
                    LoadingPanel.Instance.SetProgressText(string.Format(LauncherLocalizationManager.Instance().GetText("UI_Loading_Update"), formatStr));
                };
                //当下载器结束（无论成功或失败）
                downloader.OnDownloadOverCallback += succeed =>
                {
                    if (succeed)
                    {
                        Debug.Log($"YooAsset Download Success!");
                        
                        ChangeState<ProcedureLoadAssembly>(procedureOwner);
                    }
                    else
                    {
                        Debug.LogWarning($"YooAsset Download Fail!");
                        LauncherNoticePanel.Show(new LauncherNoticePanel.Param()
                        {
                            IsShowCloseBtn = false,
                            IsShowCancelBtn = false,
                            DescStr = LauncherLocalizationManager.Instance().GetText("UI_Desc_Download_Error"),
                            ConfirmCallBack = () =>
                            {
                                Download(procedureOwner).Forget();
                            }
                        });
                    }
                };
                downloader.BeginDownload();
            }
        }

        // private void RefreshLoadingProgress(float value)
        // {
        //     LoadingPanel.Instance.SetProgress(value);
        //     LoadingPanel.Instance.SetProgressTextFormat(LauncherLocalizationManager.Instance().GetText("UI_Loading_Update"));
        // }
        
        private static readonly double[] byteUnits =
        {
            1073741824.0, 1048576.0, 1024.0, 1
        };

        private static readonly string[] byteUnitsNames =
        {
            "GB", "MB", "KB", "B"
        };

        public static string FormatBytes(long bytes)
        {
            var size = "0 B";
            if (bytes == 0) return size;

            for (var index = 0; index < byteUnits.Length; index++)
            {
                var unit = byteUnits[index];
                if (bytes >= unit)
                {
                    size = $"{bytes / unit:##.##} {byteUnitsNames[index]}";
                    break;
                }
            }

            return size;
        }
    }
}