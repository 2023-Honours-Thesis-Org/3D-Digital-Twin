using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlModeBtn : MonoBehaviour
{
    public TMP_Text textComponent;
    private bool realTime = true;

    void Start()
    {
        realTime = true;
    }

    void Update()
    {
        switch(realTime)
        {
            case true:
                textComponent.text = "Real-Time Mode";
                break;
            case false:
                textComponent.text = "User Control Mode";
                break;
        }
    }

    public void SwitchMode()
    {
        switch(realTime)
        {
            case false:
                EventManager.InvokeRealTimeEvent();
                realTime = true;
                break;
            case true:
                EventManager.InvokeUserTime();
                realTime = false;
                break;
        }
    }
}
