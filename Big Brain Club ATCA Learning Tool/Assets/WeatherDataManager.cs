using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Controller;

namespace NetworkOperations 
{
    public class WeatherDataManager : RequestHandler
    {
        [SerializeField]
        private List<TextAsset> presetWeatherFiles;
        private const string API_URI = "api.openweathermap.org/data/2.5/weather?q=narrabri&units=metric&APPID=04105f02ff9e9ff485a8383c76cb5541";

        private string[] iconsCodes = {"01", "02", "03", "04", "09", "10", "11", "13", "50"};
        private string weatherPresetsFolder;
        private int currentWeatherCode = 0;
        private string currentDayNightCycle = "d";

        private string _currentWeather;
        public WeatherController weatherController;
        public string CurrentWeather {get; set; }

        private float timer = 0f;
        private float startTime = 0f;
        private float updateInterval = 10f;
        private bool realTimeEnabled = true;
        
        // Start is called before the first frame update
        void Start()
        {
            Result = "";
            StartCoroutine(GetRequest(API_URI));
            weatherPresetsFolder = "Assets/WeatherPresets/";
            // weatherPresetsFolder = Application.dataPath + "/WeatherPresets/";
            EventManager.RealTimeEvent += TurnOn;
            EventManager.UserTimeEvent += TurnOff;
        }

        private void TurnOff()
        {
            realTimeEnabled = false;
            UpdatePresetWeather();
        }

        private void TurnOn()
        {
            realTimeEnabled = true;
            UpdateRealWeather();
        }

        public void IncrementWeatherCode()
        {
            currentWeatherCode++;
            currentWeatherCode %= iconsCodes.Length;
            UpdatePresetWeather();
        }

        public void DecrementWeatherCode()
        {
            currentWeatherCode += iconsCodes.Length - 1;
            currentWeatherCode %= iconsCodes.Length;
            UpdatePresetWeather();
        }

        public void UpdatePresetWeather() 
        {
            string jsonFile = weatherPresetsFolder + iconsCodes[currentWeatherCode] + currentDayNightCycle + ".json";
            TextAsset data = GetWeatherPreset(iconsCodes[currentWeatherCode] + currentDayNightCycle);
            string jsonText = (data.text);
            // string jsonText = File.ReadAllText(jsonFile);
            weatherController.SetWeatherData(jsonText);
        }

        private TextAsset GetWeatherPreset(string presetCode)
        {
            switch(presetCode)
            {
                case "01d":
                    return presetWeatherFiles[0];
                    break;
                case "01n":
                    return presetWeatherFiles[1];
                    break;
                case "02d":
                    return presetWeatherFiles[2];
                    break;
                case "02n":
                    return presetWeatherFiles[3];
                    break;
                case "03d":
                    return presetWeatherFiles[4];
                    break;
                case "03n":
                    return presetWeatherFiles[5];
                    break;
                case "04d":
                    return presetWeatherFiles[6];
                    break;
                case "04n":
                    return presetWeatherFiles[7];
                    break;
                case "09d":
                    return presetWeatherFiles[8];
                    break;
                case "09n":
                    return presetWeatherFiles[9];
                    break;
                case "10d":
                    return presetWeatherFiles[10];
                    break;
                case "10n":
                    return presetWeatherFiles[11];
                    break;
                case "11d":
                    return presetWeatherFiles[12];
                    break;
                case "11n":
                    return presetWeatherFiles[13];
                    break;
                case "13d":
                    return presetWeatherFiles[14];
                    break;
                case "13n":
                    return presetWeatherFiles[15];
                    break;
                case "50d":
                    return presetWeatherFiles[16];
                    break;
                case "50n":
                    return presetWeatherFiles[17];
                    break;
            }

            return presetWeatherFiles[0];
        }

        public void UpdateRealWeather() {
            StartCoroutine(GetRequest(API_URI));
            weatherController.SetWeatherData(Result);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            float seconds = timer % 100;
            if ((seconds - startTime) >= updateInterval)
            {
                switch(realTimeEnabled)
                {
                    case true:
                        UpdateRealWeather();
                        break;
                    case false:
                        UpdatePresetWeather();
                        break;
                }
                //Debug.Log("Weather Data : " + Result.Substring(1,10));
                startTime = seconds;
            }
            
        }

        private void OnDisable()
        {
            EventManager.RealTimeEvent -= TurnOn;
            EventManager.UserTimeEvent -= TurnOff;
        }
    }
}
