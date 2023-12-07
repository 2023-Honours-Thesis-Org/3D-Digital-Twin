using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Telescope
{
    public class UserTelescopePositionBox : TelescopePositionBox
    {
        [SerializeField] public TMP_InputField[] azimuthText = new TMP_InputField[6];
        [SerializeField] public TMP_InputField[] elevationText = new TMP_InputField[6];
        [SerializeField] public TMP_InputField[] stationText = new TMP_InputField[5];

        public override void SetAzimuthText(int telescope, string azimuth)
        {
            try
            {
                azimuthText[telescope - 1].text = azimuth;
            }
            catch (IndexOutOfRangeException iore)
            {
                Debug.LogError("Cannot change element, index out of bounds :: " + iore.Message + " ::");
            }
        }

        public override void SetElevationText(int telescope, string elevation)
        {
            try
            {
                elevationText[telescope - 1].text = elevation;
            }
            catch (IndexOutOfRangeException iore)
            {
                Debug.LogError("Cannot change element, index out of bounds :: " + iore.Message + " ::");
            }
        }

        public override void SetFrequencyBandText(int telescope, string frequency) {}

        public override void SetStationText(int telescope, string station)
        {
            try
            {
                stationText[telescope - 1].text = station;
            }
            catch (IndexOutOfRangeException iore)
            {
                Debug.LogError("Cannot change element, index out of bounds :: " + iore.Message + " ::");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
