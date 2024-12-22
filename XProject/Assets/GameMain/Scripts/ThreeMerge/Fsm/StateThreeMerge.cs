using System.Threading.Tasks;
using Framework;
using ThreeMerge;

public class FsmParamTMerge : FsmParam
{
    public int level;
}
public class StateThreeMerge : IFsmState
{
    FsmStateType IFsmState.Type => FsmStateType.ThreeMerge;
    private FsmParamTMerge fsmParamTMatch;
    
    private ThreeMergeSystem tMergeSystem;
    public async Task<bool> PreEnterAsync(FsmParam param)
    {
        //UIViewSystem.Instance.Open<UITMatchMainController>();

        fsmParamTMatch = param as FsmParamTMerge;
        tMergeSystem = new ThreeMergeSystem();
        tMergeSystem.Enter(fsmParamTMatch);
        return true;
    }

    public void EnterFinish()
    {
        throw new System.NotImplementedException();
    }

    public void Update(float deltaTime)
    {
        tMergeSystem.Update(deltaTime);
    }

    public void LateUpdate(float deltaTime)
    {
        tMergeSystem.LateUpdate(deltaTime);
    }

    public void Exit(FsmStateType toStateType)
    {
        tMergeSystem.Exit();
        tMergeSystem = null;
    }
}