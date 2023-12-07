using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RealDateTimeBoxView : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;
    public GameTime gameTime;

    void Start()
    {
        timeText.text = gameTime.currentTime.ToString("HH:mm:ss") + " NSW Time";
        // dateText.text = dateTime.ToString("dddd, dd MMMM yyyy");
    }

    void Update()
    {
        timeText.text = gameTime.currentTime.ToString("HH:mm:ss") + " NSW Time";
        // dateText.text = dateTime.ToString("dddd, dd MMMM yyyy");
    }
}
