using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
//using System.Diagnostics;
using System.Net;

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.
namespace Weather {
    public class RealTimeWeather : MonoBehaviour
    {

        const string DEG_CHAR = "�";
        const string M_SPEED_UNIT = "m/h";
        const string KM_SPEED_UNIT = "km/h";

        private string[] iconsCodes = {"01", "02", "03", "04", "09", "10", "11", "13", "50"};
        public Sprite[] weatherIcons = new Sprite[18];


        [SerializeField] public Image weatherIcon;

        [SerializeField] public TextMeshProUGUI tempratureText;
        [SerializeField] public TextMeshProUGUI windSpeedText;
        [SerializeField] public TextMeshProUGUI windDirectionText;
        [SerializeField] public TextMeshProUGUI precipirationText;

        private Texture2D weatherIconTexture = null;

        public string CurrentWeather = "";
        float timer = 0.0f;
        float startTime = 0;
        public WeatherData weatherData { get; set; }
        public float updateInterval = 10.0f;

        public string[] scale = { "2x", "4x" };

        private string icon = "";
        
        void Start()
        {
            // A correct website page.
            StartCoroutine(GetRequest("api.openweathermap.org/data/2.5/weather?q=narrabri&units=metric&APPID=04105f02ff9e9ff485a8383c76cb5541"));

        }

        public IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        CurrentWeather = webRequest.downloadHandler.text;
                        break;
                }
            }
        }

        public IEnumerator loadWeatherIcon(string icon, string scale)
        {
            // http://openweathermap.org/img/wn/10d@2x.png
            string imageURL = "http://openweathermap.org/img/wn/" + icon + "@" + scale + ".png";
            using (UnityWebRequest iconRequest = UnityWebRequestTexture.GetTexture(imageURL))
            {
                yield return iconRequest.SendWebRequest();

                

                switch (iconRequest.result) {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + iconRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(": HTTP Error: " + iconRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Success! Getting Texture!");
                        weatherIconTexture = DownloadHandlerTexture.GetContent(iconRequest);
                        break;
                }
            }

        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            float seconds = timer % 60;
            if ((seconds - startTime) >= updateInterval) {
                StartCoroutine(GetRequest("api.openweathermap.org/data/2.5/weather?q=narrabri&units=metric&APPID=04105f02ff9e9ff485a8383c76cb5541"));
                weatherData = JsonUtility.FromJson<WeatherData>(CurrentWeather);
                icon = weatherData.weather[0].icon;

                startTime = seconds;
            }

            updateFields();

        }

        private void updateFields()
        {
            
            tempratureText.text = "" + weatherData.main.temp + DEG_CHAR + "C";
            float windSpeed = weatherData.wind.speed;
            if (windSpeed < 1000)
            {
                windSpeedText.text = "" + (weatherData.wind.speed) + M_SPEED_UNIT;
            }
            else 
            {
                windSpeedText.text = "" + (weatherData.wind.speed / 1000) + KM_SPEED_UNIT;
            }

            windDirectionText.text = "" + weatherData.wind.deg + DEG_CHAR;
            switch (icon)
            {
                case "01d":
                    weatherIcon.sprite = weatherIcons[0];
                    break;
                case "01n":
                    weatherIcon.sprite = weatherIcons[1];
                    break;
                case "02d":
                    weatherIcon.sprite = weatherIcons[2];
                    break;
                case "02n":
                    weatherIcon.sprite = weatherIcons[3];
                    break;
                case "03d":
                    weatherIcon.sprite = weatherIcons[4];
                    break;
                case "03n":
                    weatherIcon.sprite = weatherIcons[5];
                    break;
                case "04d":
                    weatherIcon.sprite = weatherIcons[6];
                    break;
                case "04n":
                    weatherIcon.sprite = weatherIcons[7];
                    break;
                case "09d":
                    weatherIcon.sprite = weatherIcons[8];
                    break;
                case "09n":
                    weatherIcon.sprite = weatherIcons[9];
                    break;
                case "10d":
                    weatherIcon.sprite = weatherIcons[10];
                    break;
                case "10n":
                    weatherIcon.sprite = weatherIcons[11];
                    break;
                case "11d":
                    weatherIcon.sprite = weatherIcons[12];
                    break;
                case "11n":
                    weatherIcon.sprite = weatherIcons[13];
                    break;
                case "13d":
                    weatherIcon.sprite = weatherIcons[14];
                    break;
                case "13n":
                    weatherIcon.sprite = weatherIcons[15];
                    break;
                case "50d":
                    weatherIcon.sprite = weatherIcons[16];
                    break;
                case "50n":
                    weatherIcon.sprite = weatherIcons[17];
                    break;

            }

        }
    }
}