using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action RealTimeEvent; // invoked when in real time mode
    public static event Action UserTimeEvent; // invoked when in user time mode
    
    public static void InvokeRealTimeEvent()
    {
        // invoke action with a not null check
        RealTimeEvent?.Invoke();
    }

    public static void InvokeUserTime()
    {
        // invoke action with a not null check.
        UserTimeEvent?.Invoke();
    }
}