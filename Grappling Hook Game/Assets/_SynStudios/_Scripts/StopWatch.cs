using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch
{
    public float Seconds { get; private set; }
    bool stopped;

    public StopWatch()
    {
        Reset();
    }

    public void Update()
    {
        if (stopped) return;
        Seconds += Time.deltaTime;
    }

    public void Stop() => stopped = true;
    public void Start() => stopped = false;
    public void Reset()
    {
        Seconds = 0;
        Stop();
    }

}
