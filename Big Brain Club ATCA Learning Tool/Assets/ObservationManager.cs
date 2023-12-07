using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Telescope;
using Controller;
using Unity.VisualScripting;

[System.Serializable]
public class ObservationSession
{
    public string status = "";
    public string observation_id = "";

    public static ObservationSession CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ObservationSession>(jsonString);
    }
}

[System.Serializable]
public class ArrayConfig
{
    public string array_config;
    public float hour_angle_start;
    public float hour_angle_end;
    public int sample_rate;

    public ArrayConfig()
    {
        array_config = "";
        hour_angle_start = 0.0f;
        hour_angle_end = 0.0f;
        sample_rate = 0;
    }
}

[System.Serializable]
public class ObsOptions
{
    public float freq;
    public float src_declination;
}

[System.Serializable]
public class RawImages
{
    public string img_raw;
}

[System.Serializable]
public class ProcessedImages
{
    public string uv_cov;
    public string fft_cov;
    public string obs_fft_cov;
    public string synth_beam;
    public string final_img;
}

[System.Serializable]
public class ObservationQueue
{
    public string obsid;
    public List<ArrayConfig> arrayConfigs = new();
    public List<string> stations = new();
    public Vector3 starPoint = new();

    public ObservationQueue()
    {
        this.arrayConfigs = new();
        this.stations = new();
        this.starPoint = Vector3.zero;
    }

    public void RegisterStation(string stationName)
    {
        stations.Add(stationName);
    }

    public void RegisterArrayConfig(ArrayConfig config)
    {
        arrayConfigs.Add(config);
    }

    public void RegisterStartPoint(Vector3 starPoint)
    {
        this.starPoint = starPoint;
    }
}

public class ObservationManager : MonoBehaviour
{
    const string HOST = "http://localhost:8080/api/v1/";
    const string CONTENT = "application/json";
    public ObservationSession observationInfo;
    public StarController starController;
    public GameTime gameTime;
    
    [SerializeField]
    private RawImage uvCoverage; // uv coverage of selected configuration
    [SerializeField]
    private RawImage finalImage; // final image
    [SerializeField]
    private RawImage obsFFT; // observed fft
    [SerializeField]
    private RawImage synthBeam; // synthesised beam
    [SerializeField]
    private RenderTexture image; // image to sample
    [SerializeField]
    private TelescopeController telescopeController;// To control antenna movement

    public Vector2 oldEQ = new();
    public Vector2 newEQ = new();
    public ObservationQueue obsQueue;
    
    public void RegisterStation(string stationName)
    {
        obsQueue.stations.Add(stationName);
    }

    public void StartObs()
    {
        StartCoroutine(StartObservationSession());
    }

    public void AddConfig(ArrayConfig config)
    {
        StartCoroutine(AddTelescopeConfigurations(config));
    }

    public void UvCoverage()
    {
        // StartCoroutine(SetObservationOptions(1500.0f, 300.0f));
        StartCoroutine(StartUVCov());
    }

    public void ConfigOptions(float freq, float src_declination)
    {
        StartCoroutine(SetObservationOptions(freq, src_declination));
    }

    public void FinalImage()
    {
        StartCoroutine(ObserveImage());
    }
    
    public IEnumerator StartObservationSession()
    {
        
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(HOST + "observe/init",
                                                          ""))
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();
    
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error + "::" + www.downloadHandler.text);
            }
            else
            {
                observationInfo = ObservationSession
                                    .CreateFromJSON(www.downloadHandler.text);
                Debug.Log("Obs ID: " + observationInfo.observation_id);
                obsQueue.obsid = observationInfo.observation_id;
            }
        }
    }

    public IEnumerator AddTelescopeConfigurations(ArrayConfig telescopeConfig)
    {   
        string jsonForm = JsonUtility.ToJson(telescopeConfig);
        Debug.Log(jsonForm);

        using (UnityWebRequest www = UnityWebRequest
                                     .Post(HOST + "observe/select/" +
                                           observationInfo.observation_id + 
                                           "/array", jsonForm, CONTENT))
        {
            // www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error + "::" + www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator SetObservationOptions(float freq, float src_declination)
    {
        ObsOptions options = new ObsOptions();
        options.freq = freq;
        options.src_declination = src_declination;
        
        string jsonOptions = JsonUtility.ToJson(options);

        using (UnityWebRequest www = UnityWebRequest
                                     .Post(HOST + "observe/select/" +
                                           observationInfo.observation_id + 
                                           "/options", jsonOptions, CONTENT))
        {
            // www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error + "::" + www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator StartUVCov()
    {
        using (UnityWebRequest www = UnityWebRequestTexture
                                        .GetTexture(HOST + "observe/" +
                                           observationInfo.observation_id + 
                                           "/start/uv_cov"))
        {
            
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error + "::" + www.downloadHandler.text);
            }
            else
            {
                // Debug.Log(www.downloadHandler.data);
                LoadImages(www.downloadHandler.data, uvCoverage);
                // uvCoverage.texture = ((DownloadHandlerTexture) www.downloadHandler).texture;
            }
        }
    }

    private void LoadImages(byte[] data, RawImage image)
    {
        Texture2D tex = new Texture2D(256, 148, TextureFormat.RGBA32, false);
        tex.LoadImage(data);

        image.texture = tex;
    }
    public ProcessedImages images;
    public IEnumerator ObserveImage()
    {
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
        RenderTexture.active = image;
        tex.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
        tex.Apply();

        byte[] encodedImage = tex.EncodeToPNG();
        string sentImage = System.Text.Encoding.UTF8.GetString(encodedImage);

        using (UnityWebRequest www = UnityWebRequest.Put(HOST + "observe/" + 
            observationInfo.observation_id + "/start/image", encodedImage))
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error + "::" + www.downloadHandler.text);
            }
            else
            {
                images = JsonUtility.FromJson<ProcessedImages>(www.downloadHandler.text);
                byte[] obs_fft = System.Convert.FromBase64String(images.obs_fft_cov);
                byte[] fft_cov = System.Convert.FromBase64String(images.fft_cov);
                byte[] uv_cov = System.Convert.FromBase64String(images.uv_cov);
                // Debug.Log(images.uv_cov);
                byte[] synth_beam = System.Convert.FromBase64String(images.synth_beam);
                byte[] final_img = System.Convert.FromBase64String(images.final_img);

                LoadImages(uv_cov, uvCoverage);
                LoadImages(obs_fft, obsFFT);
                LoadImages(synth_beam, synthBeam);
                LoadImages(final_img, finalImage);

                using (UnityWebRequest www2 = UnityWebRequest.Post(HOST + "observe/" + 
                       observationInfo.observation_id + "/clear", "{}", CONTENT))
                {
                    www2.downloadHandler = new DownloadHandlerBuffer();

                    yield return www2.SendWebRequest();

                    if (www2.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(www.error + "::" + www2.downloadHandler.text);
                    }
                    else
                    {
                        Debug.Log(www2.downloadHandler.text);
                    }
                }
            }
        }
    }

    public void RunQueue()
    {
        StartCoroutine(WaitForHARange());
    }

    public float timeDiff = 0.0f;
    public string _station = "";

    public TextMeshProUGUI status;
    public GameObject presetSelection;
    public GameObject targetSelection;
    public IEnumerator WaitForHARange()
    {
        status.text = "Observing";
        targetSelection.SetActive(false);
        presetSelection.SetActive(false);
        var stations = obsQueue.stations;
        var starPoint = obsQueue.starPoint;
        var configs = obsQueue.arrayConfigs;
        float speedFactor = gameTime.speedFactor;

        foreach(var station in stations) 
        {
            int arrayIndex = stations.IndexOf(station);
            float hour_angle_end = configs[arrayIndex].hour_angle_end;
            float hour_angle_start = configs[arrayIndex].hour_angle_start;
            float timeDiffRealHours = Mathf.Abs(hour_angle_end - hour_angle_start); // Time difference in real world hours
            float timeDiffGameSeconds = 0.0f;
            speedFactor = gameTime.speedFactor;
            gameTime.HAtoTime(hour_angle_start);
            
            if (speedFactor > 0) {
                timeDiffGameSeconds = (float) (timeDiffRealHours*3600.0f) / speedFactor;
            }
            else
            {
                timeDiffGameSeconds = timeDiffRealHours;
            }
            timeDiff = timeDiffGameSeconds;
            _station = station;

            var currentPointEq = Coordinates.Sky2EquatorialSky(starPoint, 
                                                                   true);

            telescopeController.SetTelescopeConfiguration(station);
            telescopeController.SetTelescopeSkyCoordinates(starPoint);
            for (int i = 0; i < timeDiffGameSeconds; i++)
            {
                
                oldEQ = Coordinates.Sky2EquatorialSky(starPoint, true);
                yield return new WaitForSeconds(1.0f/speedFactor);

                var currentPointSky = starController.UpdatePosition(gameTime.currentTime
                                                                    , currentPointEq);
                currentPointEq = Coordinates.Sky2EquatorialSky(currentPointSky, true);
                newEQ = currentPointEq;
                telescopeController.SetTelescopeSkyCoordinates(currentPointSky);
                speedFactor = gameTime.speedFactor;

                if (speedFactor > 0) {
                    timeDiffGameSeconds = (float) (timeDiffRealHours*3600.0f) / speedFactor;
                }
                else
                {
                    timeDiffGameSeconds = timeDiffRealHours;
                }
                timeDiff = timeDiffGameSeconds;
                _station = station;
            }
            
            // telescopeController.WaitForAnimations();
        }

        yield return new WaitForSeconds(1.0f/speedFactor);
        obsQueue = new();
        status.text = "Configuring";
        targetSelection.SetActive(true);
        presetSelection.SetActive(true);
    }
}
