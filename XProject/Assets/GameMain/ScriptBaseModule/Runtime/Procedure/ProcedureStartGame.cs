using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;

namespace BaseModule
{
    public class ProcedureStartGame : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            StartGame().Forget();
        }

        private async UniTaskVoid StartGame()
        {
            await UniTask.Yield();
            
            LoadingPanel.Hide();
            var _ = AtlasConfigController.Instance;//触发图集信息加载
            GameModule.Scene.LoadScene("Assets/Scenes/InitScene");
        }
    }
}