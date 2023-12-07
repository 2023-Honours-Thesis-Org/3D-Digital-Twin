using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TelescopeContextMenu
{
    public class TelescopeContextData : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI azimuthText;
        [SerializeField] private TextMeshProUGUI elevationText;
        [SerializeField] private TextMeshProUGUI stationText;
        [SerializeField] private RawImage telescopePreview;

        public string telescope;

        public void SetTelescope(string telescope)
        {
            this.telescope = telescope;
        }

        public void SetAzimuthText(string azimuth)
        {
            this.azimuthText.text = "   Azimuth: " + azimuth;
        }

        public void SetElevationText(string elevation)
        {
            this.elevationText.text = "   Elevation: " + elevation;
        }

        public void SetStationText(string station)
        {
            this.stationText.text = "   Station: " + station;
        }

        public void SetTelescopePreview(RenderTexture telescopeView)
        {
            this.telescopePreview.texture = telescopeView;
        }
    }
}
