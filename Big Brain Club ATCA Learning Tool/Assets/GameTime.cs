using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private DateTime realTime; // current real world time at ATCA site
    public DateTime currentTime; // current in game time
    private TimeZoneInfo timeZone;
    public float speedFactor = 1.0f; // the speed factor, default is 1s in game = 1s real world
    public int updateIntervalSecs = 1;

    public string currTimeString;

    // Sun and Stars
    public SunController sunController;
    public StarController starController;

    // To stop time
    public bool stopTime = false; 
    // Start is called before the first frame update
    void Start()
    {
        stopTime = false;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
        else
            timeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/NSW");
        speedFactor = 1.0f;
        realTime = DateTime.Now;
        currentTime = realTime;
        currTimeString = currentTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTime){
            StartCoroutine(UpdateCurrentTime());
        }
        sunController.UpdatePosition(currentTime);
        starController.UpdatePosition(currentTime);
    }

    public void ToggleStop()
    {
        stopTime = !stopTime;
    }

    public void SetSpeedFactor(float speedMultiplier)
    {
        stopTime = false;
        speedFactor += speedMultiplier;
    }

    // Simple function to conver selected hour angle start to in game date time.
    // example -5.6 = 05:36:00 AM & 5.6 = 05:35 PM with 0 being noon.
    public void HAtoTime(float HA)
    {
        float newHours = 0.0f;
        if (HA < 0) {
            newHours = 0 - HA;
        } else {
            newHours = HA + 12;
        }

        float timeInHours = currentTime.Hour + 
                              (currentTime.Minute/60) + 
                              (currentTime.Second/3600);
        
        float timeDiff = 12 - timeInHours;

        currentTime = currentTime.AddHours(timeDiff);
        currentTime = currentTime.AddHours(HA);

        sunController.UpdatePosition(currentTime);
        starController.UpdatePosition(currentTime);
    }

    private IEnumerator UpdateCurrentTime()
    {
        yield return new WaitForSeconds(updateIntervalSecs);
        currentTime = currentTime.AddSeconds(speedFactor);
        currTimeString = currentTime.ToString();
    }
}
