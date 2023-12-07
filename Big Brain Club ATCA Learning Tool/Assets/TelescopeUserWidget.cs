using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Controller;

namespace Telescope
{
    public class TelescopeUserWidget : MonoBehaviour
    {
        public int telescopeNumber;
        public TelescopeController telescopeController;
        
        public void setAzimuthTo(string azimuth)
        {
            telescopeController.SetTelescopeAzimuth(telescopeNumber, azimuth);
        }
        
        public void setElevationTo(string elevation)
        {
            telescopeController.SetTelescopeElevation(telescopeNumber, elevation);
        }

        public void setStationTo(string station)
        {
            telescopeController.SetTelescopeStation(telescopeNumber, station);
        }
    }
}
