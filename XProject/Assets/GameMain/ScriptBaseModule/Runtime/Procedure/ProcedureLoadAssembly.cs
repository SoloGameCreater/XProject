using System.Linq;
using System.Reflection;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace BaseModule
{
    /// <summary>
    /// 流程加载器 - 代码初始化
    /// </summary>
    public class ProcedureLoadAssembly : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            LoadHybrid();
            
            ChangeState<ProcedureStartGame>(procedureOwner);
        }
        
        private void LoadHybrid()
        {
            //不需要热更
        }
    }
}