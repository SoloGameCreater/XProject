using System.Threading.Tasks;
using Framework;
using TripleMerge;

public class FsmParamTMerge : FsmParam
{
    public int level;
}
public class StateTripleMerge : IFsmState
{
    FsmStateType IFsmState.Type => FsmStateType.TripleMerge;
    private FsmParamTMerge fsmParamTMatch;
    
    public async Task<bool> PreEnterAsync(FsmParam param)
    {
        //UIViewSystem.Instance.Open<UITMatchMainController>();
        TripleMergeSystem.Instance.OnEnterTripleMerge();
        return true;
    }

    public void EnterFinish()
    {
        UILoadingController.Hide(false);
        if (UIViewSystem.Instance.Get<LobbyMainUI>() != null)
        {
            UIViewSystem.Instance.Get<LobbyMainUI>().Show();
        }
        else
        {
            UIViewSystem.Instance.Open<LobbyMainUI>();
        }
    }

    public void Update(float deltaTime)
    {
        
    }

    public void LateUpdate(float deltaTime)
    {
        
    }

    public void Exit(FsmStateType toStateType)
    {
        TripleMergeSystem.Instance.Exit();
    }
}