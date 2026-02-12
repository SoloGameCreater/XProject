using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace BaseModule
{
    public class ProcedureInitPackage : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            InitPackage(procedureOwner).Forget();
        }

        private async UniTaskVoid InitPackage(ProcedureOwner procedureOwner)
        {
            LoadingPanel.Show();
            LoadingPanel.Instance.SetProgressTextFormat(LauncherLocalizationManager.Instance().GetText("UI_Loading_Init"));
            LoadingPanel.Instance.SetProgress(0.1f);

            await UniTask.WaitForSeconds(0.1f);

            try
            {
                var initializationOperation = await GameModule.Resource.InitPackage();

                if (initializationOperation == null || initializationOperation.Status != EOperationStatus.Succeed)
                {
                    OnInitPackageFailed(procedureOwner);
                }
                else
                {
                    LoadingPanel.Instance.SetProgress(1.0f);

                    await UniTask.Yield();
                    ChangeState<ProcedureLoadAssembly>(procedureOwner);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"InitPackage Exception: {e}");
                OnInitPackageFailed(procedureOwner);
            }
        }

        private void OnInitPackageFailed(ProcedureOwner procedureOwner)
        {
            LauncherNoticePanel.Show(new LauncherNoticePanel.Param()
            {
                IsShowCloseBtn = false,
                IsShowCancelBtn = false,
                DescStr = LauncherLocalizationManager.Instance().GetText("UI_Desc_ResPackage_Error"),
                ConfirmCallBack = () =>
                {
                    InitPackage(procedureOwner).Forget();
                }
            });
        }
    }
}
