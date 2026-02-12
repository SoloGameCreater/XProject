using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace BaseModule
{
    public class ProcedureUpdateVersion : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.LogWarning("单机模式：跳过远端资源版本更新流程。");
            ChangeState<ProcedureLoadAssembly>(procedureOwner);
        }
    }
}
