using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Telescope
{
    public class TelescopeData
    {

        private string _telescopeID;
        public string TelescopeID
        {
            get => _telescopeID;
            set => _telescopeID = value;
        }

        private float _azimuth;
        public float Azimuth 
        { 
            get => _azimuth;
            set => _azimuth = value;
        }

        private float _elevation;
        public float Elevation
        {
            get => _elevation;
            set => _elevation = value;
        }

        private float _freq;
        public float Frequency
        {
            get => _freq;
            set => _freq = value;
        }

        private string _telescopePosition;
        public string TelescopePosition
        {
            get => _telescopePosition;
            set => _telescopePosition = value;
        }

        public TelescopeData(string telescopeID)
        {
            TelescopeID = telescopeID;
            Azimuth = 0f;
            Elevation = 0f;
            TelescopePosition = "";
        }

        public string ToString()
        {
            return "Telescope(" + TelescopeID + ", " + Azimuth + ", " + Elevation + ", " + TelescopePosition + ")";
        }
    }
}
