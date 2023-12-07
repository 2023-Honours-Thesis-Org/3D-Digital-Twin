using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weather;

namespace Controller
{
    public class WeatherController : MonoBehaviour
    {
        private WeatherData weatherData = null;
        
        private bool isNull = true;

        public WeatherBoxView realWeatherBox;
        public WeatherBoxView userWeatherBox;
        private WeatherBoxView currentWeatherBox;
        public CloudController cloudController;
        public RainController rainController;
        public LightningController lightningController;

        void Start()
        {
            EventManager.UserTimeEvent += UserTimeSetup;
            EventManager.RealTimeEvent += RealTimeSetup;
            userWeatherBox.DisableWidget();
            realWeatherBox.EnableWidget();
            currentWeatherBox = realWeatherBox;
        }

        public void SetWeatherData(string weatherResult)
        {
            if (!weatherResult.Equals("")) {
                weatherData = JsonUtility.FromJson<WeatherData>(weatherResult);
                UpdateUI();
                UpdateClouds();
            }
        }

        private void UserTimeSetup()
        {
            realWeatherBox.DisableWidget();
            userWeatherBox.EnableWidget();
            currentWeatherBox = userWeatherBox;
        }

        private void RealTimeSetup()
        {
            userWeatherBox.DisableWidget();
            realWeatherBox.EnableWidget();
            currentWeatherBox = realWeatherBox;
        }

        private void UpdateUI() 
        {
            if (weatherData != null)
            {
                currentWeatherBox.SetTemparetureText(weatherData.main.temp);
                currentWeatherBox.SetWindSpeed(weatherData.wind.speed);
                currentWeatherBox.SetWindDirection(weatherData.wind.deg);
                currentWeatherBox.SetWeatherIcon(weatherData.weather[0].icon);
            }
        }

        public void UpdateClouds()
        {
            if (weatherData != null)
            {
                if (weatherData.weather[0].id >= 800)
                {
                    lightningController.lightingEnabled = false;
                    // rainController.setRainSpeed(0);
                    switch (weatherData.weather[0].id)
                    {
                        case 800: cloudController.SetToClearSky(); break; // Clear Skys
                        case 801: cloudController.SetToFewClouds(); break; // Few Clouds
                        case 802: cloudController.SetToScatteredClouds(); break; // Scattered Clouds
                        case 803: cloudController.SetToBrokenClouds(); break; // Broken Clouds
                        case 804: cloudController.SetToOvercast(); break; // Overcast
                    }
                }
                else
                {
                    switch(weatherData.weather[0].main)
                    {
                        case "Rain":
                            cloudController.SetToRain();
                            lightningController.lightingEnabled = false;
                            // rainController.setRainSpeed(100);
                            break;
                        case "Thunderstorm":
                            cloudController.SetToThunderstorm();
                            lightningController.lightingEnabled = true;
                            // rainController.setRainSpeed(0);
                            break;
                        case "Drizzle":
                            cloudController.SetToDrizzle();
                            lightningController.lightingEnabled = false;
                            // rainController.setRainSpeed(50);
                            break;
                    }
                }
            }
        }

        private void OnDisable()
        {
            EventManager.RealTimeEvent -= RealTimeSetup;
            EventManager.UserTimeEvent -= UserTimeSetup;
        }
    }
}