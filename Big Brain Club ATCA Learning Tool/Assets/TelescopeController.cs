using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using System.Text.RegularExpressions;
using Telescope;

namespace Controller
{
    public class TelescopeController : MonoBehaviour
    {
        private Dictionary<string, TelescopeData> telescopes = new Dictionary<string, TelescopeData>();
        public TelescopePositionBox realTelescopePositionBox;
        public TelescopePositionBox userTelescopePositionBox;
        private TelescopePositionBox currentTelescopePositionBox;
        public TelescopePointData telescopePointData;
        public ConfigurationBox configurationBox;
        public CoordinatesBox coordinatesBox;
        private bool realTimeEnabled = true;
        public AntennaController antennaController;

        void Start()
        {
            // configurationBox.DisableWidget();
            // coordinatesBox.DisableWidget();
            userTelescopePositionBox.DisableWidget();
            currentTelescopePositionBox = realTelescopePositionBox;
            EventManager.UserTimeEvent += UserTimeSetup;
            EventManager.RealTimeEvent += RealTimeSetup;
            for (int i = 1; i <= 6; i++)
            {
                telescopes.Add("ca0"+i, new TelescopeData("ca0"+i));
            }
        }

        public void WaitForAnimations()
        {
            antennaController.allAntennaCompleteAnimations();
        }
        public void SetTelescopeConfiguration(string config)
        {
            antennaController.setArrayConfigurationTo(config);
        }
        public void SetTelescopeData(string telescopePage)
        {
            if (telescopePage != null && telescopePage != "")
            {
                telescopePointData = JsonUtility.FromJson<TelescopePointData>(telescopePage);
                Debug.Log(telescopePointData.pointData[0].label);
                //UpdateModel(telescopePage);
                UpdateUI();
            }
        }

        public void SetTelescopeAzimuth(int telescopeNumber, string azimuth)
        {
            float azimuthFloat = float.Parse(azimuth);
            ((GameObject)(antennaController.antennaClones[telescopeNumber - 1])).GetComponent<ATCAantenna>().setAzimuthTo(azimuthFloat);
            currentTelescopePositionBox.SetAzimuthText(telescopeNumber, azimuth);
        }

        public void SetTelescopeAzimuth(int telescopeNumber, double azimuth, string azimuthText)
        {
            ((GameObject)(antennaController.antennaClones[telescopeNumber])).GetComponent<ATCAantenna>().setAzimuthTo((float) azimuth);
        }

        public void SetTelescopeElevation(int telescopeNumber, string elevation)
        {
            float elevationFloat = float.Parse(elevation);
            ((GameObject)(antennaController.antennaClones[telescopeNumber - 1])).GetComponent<ATCAantenna>().setElevationTo(elevationFloat);
            currentTelescopePositionBox.SetElevationText(telescopeNumber, elevation);
        }

        public void SetTelescopeElevation(int telescopeNumber, double elevation, string elevationText)
        {
            ((GameObject)(antennaController.antennaClones[telescopeNumber])).GetComponent<ATCAantenna>().setElevationTo((float) elevation);
        }

        public void SetTelescopeStation(int telescopeNumber, string station)
        {
            if (station != "W392")
            {
                antennaController.moveAntennaTo(telescopeNumber, station);
                currentTelescopePositionBox.SetStationText(telescopeNumber, station);
            }
        }

        public void SetTelescopeSkyCoordinates(Vector3 skyCoords)
        {
            for (int i = 0; i < 6; i++)
            {
                ((GameObject)(antennaController.antennaClones[i])).GetComponent<ATCAantenna>().pointToCoordinates(skyCoords);
            }
        }

        private void UpdateModel(string telescopePage)
        {
            string azimuthString = Regex.Match(telescopePage, @"azimuth of [\d.]+ degrees").Value;
            float azimuth = float.Parse(Regex.Match(azimuthString, @"[\d.]+").Value);

            string elevationString = Regex.Match(telescopePage, @"elevation of [\d.]+ degrees").Value;
            float elevation = float.Parse(Regex.Match(elevationString, @"[\d.]+").Value);

            MatchCollection imageFilesMatch = Regex.Matches(telescopePage, "live-images/(.*)\"");
            MatchCollection telescopeIDMatch = Regex.Matches(telescopePage, "live-images/ca(.*)\"");
            string[] images = new string[6];
            string[] idImages = new string[6];

            for (int i = 0; i < 6; i++)
            {
                string imageName;
                string telescopeCode;
                imageName = imageFilesMatch[i].Value.Replace("live-images/", "").Replace("\"", "");
                telescopeCode = telescopeIDMatch[i].Value.Replace("live-images/", "").Replace("\"", "");
            
                telescopes[telescopeCode.Substring(0, 4)].TelescopePosition = imageName.Substring(0, 2);
                telescopes[telescopeCode.Substring(0, 4)].Azimuth = azimuth;
                telescopes[telescopeCode.Substring(0, 4)].Elevation = elevation;

                //Debug.Log(telescopes[telescopeCode.Substring(0, 4)].ToString());
            }
        }

        

        private void UpdateUI()
        {
            foreach (PointData point in telescopePointData.pointData)
            {
                bool errorState = point.errorState;
                string value = point.value;
                string label = point.label;
                string time = point.time;
                string pointName = point.pointName;
                string telescope = pointName.Substring(0, 4);
                int telescopeNumber = 0;

                switch (label)
                {
                    case "Azimuth":
                        

                        switch (telescope)
                        {
                            case "ca01":
                                telescopeNumber = 0;
                                break;
                            case "ca02":
                                telescopeNumber = 1;
                                break;
                            case "ca03":
                                telescopeNumber = 2;
                                break;
                            case "ca04":
                                telescopeNumber = 3;
                                break;
                            case "ca05":
                                telescopeNumber = 4;
                                break;
                            case "ca06":
                                telescopeNumber = 5;
                                break;
                        }
                        currentTelescopePositionBox.SetAzimuthText(telescopeNumber, value);

                        string[] AzValue = value.Split(new char[3] {'\u00B0', '\'', '\"'});
                        try
                        {
                            double degrees = double.Parse(AzValue[0]);
                            double minutes = double.Parse(AzValue[1]);
                            double seconds = double.Parse(AzValue[2] + AzValue[3]);
                            
                            double azimuth = Coordinates.sexagesimalToDecimalDegrees(degrees, minutes, seconds);
                            SetTelescopeAzimuth(telescopeNumber, azimuth, value);
                        } catch (FormatException fe)
                        {
                            Debug.Log("Null values found, cannot change azimuth");
                        }
                        break;
                    case "Elevation":
                        switch (telescope)
                        {
                            case "ca01":
                                telescopeNumber = 0;
                                break;
                            case "ca02":
                                telescopeNumber = 1;
                                break;
                            case "ca03":
                                telescopeNumber = 2;
                                break;
                            case "ca04":
                                telescopeNumber = 3;
                                break;
                            case "ca05":
                                telescopeNumber = 4;
                                break;
                            case "ca06":
                                telescopeNumber = 5;
                                break;
                        }
                        currentTelescopePositionBox.SetElevationText(telescopeNumber, value);

                        try
                        {
                            string[] ElValue = value.Split(new char[3] {'\u00B0', '\'', '\"'});
                            double eldegrees = double.Parse(ElValue[0]);
                            double elminutes = double.Parse(ElValue[1]);
                            double elseconds = double.Parse(ElValue[2] + ElValue[3]);
                            
                            double elevation = Coordinates.sexagesimalToDecimalDegrees(eldegrees, elminutes, elseconds);
                            SetTelescopeElevation(telescopeNumber, elevation, value);
                        } catch (FormatException fe)
                        {
                            Debug.Log("Null values found, cannot change elevation");
                        }
                        break;
                    case "Feed":
                        switch (telescope)
                        {
                            case "ca01":
                                telescopeNumber = 0;
                                break;
                            case "ca02":
                                telescopeNumber = 1;
                                break;
                            case "ca03":
                                telescopeNumber = 2;
                                break;
                            case "ca04":
                                telescopeNumber = 3;
                                break;
                            case "ca05":
                                telescopeNumber = 4;
                                break;
                            case "ca06":
                                telescopeNumber = 5;
                                break;
                        }
                        currentTelescopePositionBox.SetFrequencyBandText(telescopeNumber, value);
                        break;
                }
            }
        }

        private void UserTimeSetup()
        {
            //realTelescopePositionBox.DisableWidget();
            //userTelescopePositionBox.EnableWidget();
            //currentTelescopePositionBox = userTelescopePositionBox;
            //configurationBox.EnableWidget();
            //coordinatesBox.EnableWidget();
            realTimeEnabled = false;
        }

        private void RealTimeSetup()
        {
            //userTelescopePositionBox.DisableWidget();
            //realTelescopePositionBox.EnableWidget();
            //currentTelescopePositionBox = realTelescopePositionBox;
            //configurationBox.DisableWidget();
            //coordinatesBox.DisableWidget();
            realTimeEnabled = true;
        }
    }
}