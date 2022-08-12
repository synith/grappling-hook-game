using System;
using UnityEngine;

public class ActionTimer
{
    Action action;
    float timer;

    public ActionTimer(Action action, float timer)
    {
        this.action = action;
        this.timer = timer;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            action();         
        }
    }
}
