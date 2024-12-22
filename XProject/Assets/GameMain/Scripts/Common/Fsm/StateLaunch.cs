using System.Threading.Tasks;
using Framework;

public class StateLaunch : IFsmState
{
    FsmStateType IFsmState.Type => FsmStateType.Launch;

    public Launching _launching;

    public async Task<bool> PreEnterAsync(FsmParam param)
    {
        _launching = new Launching();
        
        return true;
    }

    void IFsmState.EnterFinish()
    {
    }

    public void Update(float deltaTime)
    {
        _launching.Update();
    }
    
    public void LateUpdate(float deltaTime)
    {
        
    }

    public void Exit(FsmStateType toStateType)
    {
    }
}