
using System;
using Framework;
using UnityEngine;

public class TimeTickModel : GlobalSystem<TimeTickModel>, IUpdatable
{
    private Action _tick_func;
    private float time;

    public void Update(float deltaTime)
    {
        time += deltaTime;
        if (time < 1) return;

        time -= 1;
        if (_tick_func != null)
            _tick_func();
    }

    public void Register(Action func)
    {
        if (func == null) return;
        func.Invoke();
        if (_tick_func == null)
        {
            _tick_func = func;
            return;
        }

        _tick_func += func;
    }

    public void Remove(Action func)
    {
        if (_tick_func == null) return;
        _tick_func -= func;
    }
}