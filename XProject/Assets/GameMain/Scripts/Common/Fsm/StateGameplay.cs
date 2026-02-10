using System.Threading.Tasks;
using Framework;
using GameplayRuntime;

public class StateGameplay : IFsmState
{
    FsmStateType IFsmState.Type => FsmStateType.Gameplay;

    public async Task<bool> PreEnterAsync(FsmParam param)
    {
        return await GameplayDirector.Instance.EnterDefaultAsync(param);
    }

    public void EnterFinish()
    {
        GameplayDirector.Instance.EnterFinish();
    }

    public void Update(float deltaTime)
    {
        GameplayDirector.Instance.Update(deltaTime);
    }

    public void LateUpdate(float deltaTime)
    {
        GameplayDirector.Instance.LateUpdate(deltaTime);
    }

    public void Exit(FsmStateType toStateType)
    {
        GameplayDirector.Instance.ExitCurrentSession();
    }
}
