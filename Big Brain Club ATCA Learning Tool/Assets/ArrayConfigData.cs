using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ArrayConfigData : MonoBehaviour
{
    public ArrayConfig data;
    public TextMeshProUGUI configName;
    public Text end;
    public Text start;
    public TMP_InputField sampleRate;
    public string station;
    public ObservationManager obsMan;
    public GameTime gameTime;
    public float firstHAStart = 0.0f;

    public bool updateTime = true;
    // Start is called before the first frame update
    void Start()
    {
        data = new();
        data.array_config = "ATCA_" + configName.text;
        station = configName.text;
        data.hour_angle_end = float.Parse(end.text);
        data.hour_angle_start = float.Parse(start.text);
        data.sample_rate = int.Parse(sampleRate.text);
        updateTime = true;
        firstHAStart = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        data = new();
        data.array_config = "ATCA_" + configName.text;
        station = configName.text;
        data.hour_angle_end = float.Parse(end.text);
        data.hour_angle_start = float.Parse(start.text);
        data.sample_rate = int.Parse(sampleRate.text);
        }

    public void RegisterData()
    {
        obsMan.obsQueue.RegisterArrayConfig(data);
        obsMan.obsQueue.RegisterStation(station);
        if (updateTime)
        {
            gameTime.HAtoTime(data.hour_angle_start);
            firstHAStart = data.hour_angle_start;
            updateTime = false;
        }
    }

    public void SendConfig()
    {
        obsMan.AddConfig(data);
    }
}
