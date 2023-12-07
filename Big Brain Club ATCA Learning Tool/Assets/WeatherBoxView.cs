using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Weather
{
    public class WeatherBoxView : MonoBehaviour
    {        
        /* UI elements for weather widget */
        private const string DEG_CHAR = "\u00B0";
        private const string M_SPEED_UNIT = "m/h";
        private const string KM_SPEED_UNIT = "km/h";

        public Sprite[] weatherIcons = new Sprite[18];


        [SerializeField] public Image weatherIcon;

        [SerializeField] public TextMeshProUGUI tempratureText;
        [SerializeField] public TextMeshProUGUI windSpeedText;
        [SerializeField] public TextMeshProUGUI windDirectionText;

        public void EnableWidget()
        {
            gameObject.SetActive(true);
        }

        public void SetTemparetureText(float temprature)
        {
            tempratureText.text = "" + temprature + DEG_CHAR + "C";
        }

        public void SetWindSpeed(float windSpeed)
        {
            if (windSpeed < 1000)
            {
                windSpeedText.text = "" + windSpeed + M_SPEED_UNIT;
            }
            else
            {
                windSpeedText.text = "" + (windSpeed/1000) + KM_SPEED_UNIT;
            }
        }

        public void SetWindDirection(int direction)
        {
            windDirectionText.text = "" + direction + DEG_CHAR;
        }

        public void SetWeatherIcon(string iconCode)
        {
            switch (iconCode)
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

        public void DisableWidget()
        {
            gameObject.SetActive(false);
        }
    }
}