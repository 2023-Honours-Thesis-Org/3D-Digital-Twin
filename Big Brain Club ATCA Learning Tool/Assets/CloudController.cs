using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Weather
{
    public class CloudController : MonoBehaviour
    {
        public Volume m_Volume;
        // RealTimeWeather weatherObject;
        // Start is called before the first frame update
        void Start()
        {
            // weatherObject = GetComponent<RealTimeWeather>();
            SetToClearSky();
        }



        // Update is called once per frame
        // void Update()
        // {
        //     Debug.Log(weatherObject.weatherData.weather[0].id);
        //     Debug.Log(weatherObject.weatherData.weather[0].main);

        //     if (weatherObject.weatherData.weather[0].id >= 800)
        //     {
        //         switch (weatherObject.weatherData.weather[0].id)
        //         {
        //             case 800: SetToClearSky(); break; // Clear Skys
        //             case 801: SetToFewClouds(); break; // Few Clouds
        //             case 802: SetToScatteredClouds(); break; // Scattered Clouds
        //             case 803: SetToBrokenClouds(); break; // Broken Clouds
        //             case 804: SetToOvercast(); break; // Overcast
        //         }
        //     } else if (weatherObject.weatherData.weather[0].main == "Rain")
        //     {
        //         SetToRain();
        //     } else if (weatherObject.weatherData.weather[0].main == "Rain")
        //     {
        //         SetToRain();
        //     } else if (weatherObject.weatherData.weather[0].main == "Thunderstorm")
        //     {
        //         SetToThunderstorm();
        //     } else if (weatherObject.weatherData.weather[0].main == "Drizzle")
        //     {
        //         SetToDrizzle();
        //     }
        // }

        public void SetToFewClouds()
        {
            VolumeProfile profile = m_Volume.sharedProfile;
            profile.TryGet<VolumetricClouds>(out var volumetricClouds);
            volumetricClouds.cloudPreset = VolumetricClouds.CloudPresets.Sparse;
        }

        public void SetToOvercast()
        {
            VolumeProfile profile = m_Volume.sharedProfile;
            profile.TryGet<VolumetricClouds>(out var volumetricClouds);
            volumetricClouds.cloudPreset = VolumetricClouds.CloudPresets.Overcast;
        }

        public void SetToClearSky()
        {
            // To be used for description: Clear Sky
            SetCloudsCustom(0, 0, 0, 0, 0);
        }

        public void SetToRain()
        {
            // To be used for description: Shower Rain & Rain
            SetCloudsCustom(0.6f, 0.74f, 3, 0.73f, 60);
        }

        public void SetToThunderstorm()
        {
            // To be used for description: Thunderstorm
            SetCloudsCustom(0.75f, 0.65f, 4, 0.5f, 50);
        }

        public void SetToScatteredClouds()
        {
            // To be used for description: Scattered Clouds
            SetCloudsCustom(0.15f, 0.89f, 0.3f, 0.5f, 30);
        }

        public void SetToBrokenClouds()
        {
            SetCloudsCustom(0.15f, 0.89f, 0.6f, 1.0f, 90);
        }
        public void SetToDrizzle()
        {
            SetCloudsCustom(0.28f, 0.76f, 2.5f, 0.73f, 60);
        }
        private void SetCloudsCustom(float densityMultiplier, float shapeFactor,
                    float shapeScale, float erosionFactor, float erosionScale)
        {
            VolumeProfile profile = m_Volume.sharedProfile;
            profile.TryGet<VolumetricClouds>(out var volumetricClouds);
            if (volumetricClouds.cloudPreset != VolumetricClouds.CloudPresets.Custom)
            {
                volumetricClouds.cloudPreset = VolumetricClouds.CloudPresets.Custom;
                
                // Setting the override state to true before we change the values
                volumetricClouds.densityMultiplier.overrideState = true;
                volumetricClouds.shapeFactor.overrideState = true;
                volumetricClouds.shapeScale.overrideState = true;
                volumetricClouds.erosionFactor.overrideState = true;
                volumetricClouds.erosionScale.overrideState = true;
            }

            // Setting the values to the custom vaues given through the function
            volumetricClouds.densityMultiplier.value = densityMultiplier;
            volumetricClouds.shapeFactor.value = shapeFactor;
            volumetricClouds.shapeScale.value = shapeScale;
            volumetricClouds.erosionFactor.value = erosionFactor;
            volumetricClouds.erosionScale.value = erosionScale;
        }
    }
}