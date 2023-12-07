using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Controls;

public class RegisterData : MonoBehaviour
{
    private ArrayConfig arrayConfig = new();

    public TextMeshProUGUI station;
    public Slider negativeHA;
    public Slider positiveHA;
    public TMP_InputField sampleRate;

    public ObservationManager obsMan;
    // Start is called before the first frame update
    public void Register()
    {
        arrayConfig.array_config = station.text;
        arrayConfig.hour_angle_start = 0 - negativeHA.value;
        arrayConfig.hour_angle_end = positiveHA.value;
        arrayConfig.sample_rate = int.Parse(sampleRate.text);

        obsMan.obsQueue.RegisterStation(station.text);
        obsMan.obsQueue.RegisterArrayConfig(arrayConfig);

        Debug.Log(obsMan.obsQueue);

        arrayConfig = new();
    }
}
