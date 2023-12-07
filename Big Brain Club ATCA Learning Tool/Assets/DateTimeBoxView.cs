using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class DateTimeBoxView : MonoBehaviour
{
    public void EnableWidget()
    {
        gameObject.SetActive(true);
    }

    public void DisableWidget()
    {
        gameObject.SetActive(false);
    }

    public abstract void SetDateText(System.DateTime dateTime);

    public abstract void SetTimeText(System.DateTime dateTime);
}
