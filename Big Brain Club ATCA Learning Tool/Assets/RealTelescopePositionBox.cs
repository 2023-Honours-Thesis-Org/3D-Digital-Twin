using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Telescope
{
    public class RealTelescopePositionBox : TelescopePositionBox
    {
        [SerializeField] public Image[] telescopeIcon = new Image[6];
        [SerializeField] public TextMeshProUGUI[] azimuthText = new TextMeshProUGUI[6];
        [SerializeField] public TextMeshProUGUI[] elevationText = new TextMeshProUGUI[6];
        [SerializeField] public TextMeshProUGUI[] frequencyBandText = new TextMeshProUGUI[6];

        [SerializeField] public Sprite[] hTelescopeSprites = new Sprite[8];
        [SerializeField] public Sprite[] lTelescopeSprites = new Sprite[16];
        [SerializeField] public Sprite[] mTelescopeSprites = new Sprite[12];
        [SerializeField] public Sprite[] pTelescopeSprites = new Sprite[1];
        [SerializeField] public Sprite[] sTelescopeSprites = new Sprite[1];

        public override void SetAzimuthText(int telescope, string azimuth)
        {
            try
            {
                azimuthText[telescope].text = "Azimuth: " + azimuth;
            }
            catch(IndexOutOfRangeException iore)
            {
                Debug.LogError("Cannot change element, index out of bounds :: " + iore.Message + " ::");
            }

        }

        public override void SetElevationText(int telescope, string elevation)
        {
            try
            {
                elevationText[telescope].text = "Elevation: " + elevation;
            }
            catch(IndexOutOfRangeException iore)
            {
                Debug.LogError("Cannot change element, index out of bounds :: " + iore.Message + " ::");
            }
        }

        public override void SetFrequencyBandText(int telescope, string freqBand)
        {
            try
            {
                frequencyBandText[telescope].text = "Feed: " + freqBand;
            }
            catch(IndexOutOfRangeException iore)
            {
                Debug.LogError("Cannot change element, index out of bounds :: " + iore.Message + " ::");
            }
        }

        public override void SetStationText(int telescope, string station) {}

        public void SetTelescopeIcon(int telescope, string position)
        {
            try
            {
                
                char positionType = position[0];
                char positionIndex = position[1];

                int index = int.Parse(""+positionIndex, System.Globalization.NumberStyles.HexNumber);

                switch(positionType)
                {
                    case 'h':
                        telescopeIcon[telescope].sprite = hTelescopeSprites[index];
                        break;
                    case 'l':
                        telescopeIcon[telescope].sprite = lTelescopeSprites[index];
                        break;
                    case 'm':
                        telescopeIcon[telescope].sprite = mTelescopeSprites[index];
                        break;
                    case 'p':
                        telescopeIcon[telescope].sprite = pTelescopeSprites[index];
                        break;
                    case 's':
                        telescopeIcon[telescope].sprite = sTelescopeSprites[index];
                        break;
                }
            }
            catch (IndexOutOfRangeException iore)
            {
                Debug.LogError(telescope + ":: " + iore.Message + " ::");
            }
        }
    }
}