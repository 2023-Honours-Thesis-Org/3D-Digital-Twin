using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserDateTimeBoxView : DateTimeBoxView
{
    public TMP_InputField dateText;
    public TMP_InputField timeText;

    public override void SetDateText(System.DateTime dateTime)
    {
        dateText.text = dateTime.ToString("dddd, dd MMMM yyyy");
    }

    public override void SetTimeText(System.DateTime dateTime)
    {
        timeText.text = dateTime.ToString("HH:mm:ss");
    }
}
