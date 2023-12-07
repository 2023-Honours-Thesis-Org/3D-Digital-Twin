using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TelescopePositionBox : MonoBehaviour
{
    public void EnableWidget()
    {
        gameObject.SetActive(true);
    }

    public void DisableWidget()
    {
        gameObject.SetActive(false);
    }

    public abstract void SetAzimuthText(int telescope, string azimuth);

    public abstract void SetElevationText(int telescope, string elevation);

    public abstract void SetFrequencyBandText(int telescope, string freqBand);
    
    public abstract void SetStationText(int telescope, string freqBand);
}
