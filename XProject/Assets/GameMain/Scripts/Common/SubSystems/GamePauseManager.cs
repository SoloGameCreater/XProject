
using System;
using Framework;
using UnityEngine;
public class GamePauseManager: GlobalSystem<GamePauseManager>, IOnApplicationPause
{
    public enum PauseReasonMask : byte
    {
        BindFacebook = 1 << 0,
        LikeUs = 1 << 1,
        RateUs = 1 << 2,
        Pay = 1 << 3,
        Share = 1 << 4,
    }

    private byte _pauseReason;

    private Action _pauseCallback;

    public void AddCallback(Action callback)
    {
        _pauseCallback += callback;
    }

    public void RegisterPauseReason(PauseReasonMask mask)
    {
        _pauseReason |= (byte)mask;
    }

    private float m_PauseTime;

    public void OnApplicationPause(bool pause)
    {
        DebugUtil.Log($"OnApplicationPause, pause = {pause}");
        if (!pause)
        {
            float delta = Time.realtimeSinceStartup - m_PauseTime;
            DebugUtil.Log($"pause time : {delta},  pause reason = {_pauseReason}");

            if (_pauseCallback != null)
            {
                _pauseCallback.Invoke();
                _pauseCallback = null;
            }

            OnResumeFromBackground();
            _pauseReason = 0;
        }
        else
        {
            m_PauseTime = Time.realtimeSinceStartup;
        }

        EventDispatcher.Instance.DispatchEvent(new OnApplicationPauseEvent(pause));
    }

    public void OnResumeFromBackground()
    {
        
    }
}