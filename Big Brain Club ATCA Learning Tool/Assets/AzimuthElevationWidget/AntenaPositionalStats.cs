using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;
using TMPro;
using System.Threading;
using Weather;
using UnityEngine.UI;

namespace Telescope
{
	public class AntenaPositionalStats : MonoBehaviour
	{
        [SerializeField] public Image[] telescopeIcon = new Image[6];
        [SerializeField] public TextMeshProUGUI[] azimuthText = new TextMeshProUGUI[6];
        [SerializeField] public TextMeshProUGUI[] elevationText = new TextMeshProUGUI[6];
        [SerializeField] public TextMeshProUGUI[] frequencyBandText = new TextMeshProUGUI[6];

        // HttpClient is intended to be instantiated once per application, rather than per-use.
        // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-6.0
        static readonly HttpClient client = new HttpClient();
        public static Dictionary<String, TelescopeData> telescopes = new Dictionary<String, TelescopeData>();
        [SerializeField] public Image foo;
        public Sprite[] hTelescopeSprites = new Sprite[8];
        public Sprite[] lTelescopeSprites = new Sprite[16];
        public Sprite[] mTelescopeSprites = new Sprite[12];
        public Sprite[] pTelescopeSprites = new Sprite[1];
        public Sprite[] sTelescopeSprites = new Sprite[1];

        private string webPageResult = "";
        public string WebPageResult
        {
            get => webPageResult;
            set => webPageResult = value;
        }

        float timer = 0f;
        float startTime = 0f;
        public float updateInterval = 10.0f;
        void Start()
        {
            foo.sprite = hTelescopeSprites[0];
            for (int i = 1; i <= 6; i++)
            {
                telescopes.Add("ca0"+i, new TelescopeData("ca0"+i));
            }
        }

        void Update()
        {
            _ = Main();
            updateUI();
        }

        private void updateUI()
        {
            for (int i = 0; i < 6; i++)
            {
                string id = "ca0" + (i + 1);
                char position = (char) 0;
                char index = (char) 0;
                int positionIndex = 0;
                azimuthText[i].text = "Azimuth: " + telescopes[id].Azimuth;
                elevationText[i].text = "Elevation: " + telescopes[id].Elevation;
                Debug.Log("Position: " + telescopes[id].TelescopePosition);

               switch (telescopes[id].TelescopePosition)
                {
                    case "h0":
                        telescopeIcon[i].sprite = hTelescopeSprites[0];
                        break;
                    case "h1":
                        telescopeIcon[i].sprite = hTelescopeSprites[1];
                        break;
                    case "h2":
                        telescopeIcon[i].sprite = hTelescopeSprites[2];
                        break;
                    case "h3":
                        telescopeIcon[i].sprite = hTelescopeSprites[3];
                        break;
                    case "h4":
                        telescopeIcon[i].sprite = hTelescopeSprites[4];
                        break;
                    case "h5":
                        telescopeIcon[i].sprite = hTelescopeSprites[5];
                        break;
                    case "h6":
                        telescopeIcon[i].sprite = hTelescopeSprites[6];
                        break;
                    case "h7":
                        telescopeIcon[i].sprite = hTelescopeSprites[7];
                        break;
                    case "l0":
                        telescopeIcon[i].sprite = lTelescopeSprites[0];
                        break;
                    case "l1":
                        telescopeIcon[i].sprite = lTelescopeSprites[1];
                        break;
                    case "l2":
                        telescopeIcon[i].sprite = lTelescopeSprites[2];
                        break;
                    case "l3":
                        telescopeIcon[i].sprite = lTelescopeSprites[3];
                        break;
                    case "l4":
                        telescopeIcon[i].sprite = lTelescopeSprites[4];
                        break;
                    case "l5":
                        telescopeIcon[i].sprite = lTelescopeSprites[5];
                        break;
                    case "l6":
                        telescopeIcon[i].sprite = lTelescopeSprites[6];
                        break;
                    case "l7":
                        telescopeIcon[i].sprite = lTelescopeSprites[7];
                        break;
                    case "l8":
                        telescopeIcon[i].sprite = lTelescopeSprites[8];
                        break;
                    case "l9":
                        telescopeIcon[i].sprite = lTelescopeSprites[9];
                        break;
                    case "lA":
                        telescopeIcon[i].sprite = lTelescopeSprites[10];
                        break;
                    case "lB":
                        telescopeIcon[i].sprite = lTelescopeSprites[11];
                        break;
                    case "lC":
                        telescopeIcon[i].sprite = lTelescopeSprites[12];
                        break;
                    case "lD":
                        telescopeIcon[i].sprite = lTelescopeSprites[13];
                        break;
                    case "lE":
                        telescopeIcon[i].sprite = lTelescopeSprites[14];
                        break;
                    case "lF":
                        telescopeIcon[i].sprite = lTelescopeSprites[15];
                        break;
                    case "m0":
                        telescopeIcon[i].sprite = mTelescopeSprites[0];
                        break;
                    case "m1":
                        telescopeIcon[i].sprite = mTelescopeSprites[1];
                        break;
                    case "m2":
                        telescopeIcon[i].sprite = mTelescopeSprites[2];
                        break;
                    case "m3":
                        telescopeIcon[i].sprite = mTelescopeSprites[3];
                        break;
                    case "m4":
                        telescopeIcon[i].sprite = mTelescopeSprites[4];
                        break;
                    case "m5":
                        telescopeIcon[i].sprite = mTelescopeSprites[5];
                        break;
                    case "m6":
                        telescopeIcon[i].sprite = mTelescopeSprites[6];
                        break;
                    case "m7":
                        telescopeIcon[i].sprite = mTelescopeSprites[7];
                        break;
                    case "m8":
                        telescopeIcon[i].sprite = mTelescopeSprites[8];
                        break;
                    case "m9":
                        telescopeIcon[i].sprite = mTelescopeSprites[9];
                        break;
                    case "mA":
                        telescopeIcon[i].sprite = mTelescopeSprites[10];
                        break;
                    case "mB":
                        telescopeIcon[i].sprite = mTelescopeSprites[11];
                        break;
                    case "p0":
                        telescopeIcon[i].sprite = pTelescopeSprites[0];
                        break;
                    case "s0":
                        telescopeIcon[i].sprite = sTelescopeSprites[0];
                        break;
                }
                
            }
            
        }

        private async Task Main()
		{
            char positionType;
            int positionIndex;
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            string uri = "https://www.narrabri.atnf.csiro.au/cgi-bin/Public/atca_live.cgi";
            try
            {   
                // gets website code as string
                string responseBody = await client.GetStringAsync(uri);

                // azimuth
                string azimuth_string = Regex.Match(responseBody, @"azimuth of [\d.]+ degrees").Value;
                float azimuth = float.Parse(Regex.Match(azimuth_string, @"[\d.]+").Value);
/*                Debug.Log($"azimuth of {azimuth} degrees");
*/
                // elevation
                string elevation_string = Regex.Match(responseBody, @"elevation of [\d.]+ degrees").Value;
                float elevation = float.Parse(Regex.Match(elevation_string, @"[\d.]+").Value);
                /*Debug.Log($"elevation of {elevation} degrees");*/

                

                // get name of image files and respective telescope ID
                MatchCollection imageFilesMatch = Regex.Matches(responseBody, "live-images/(.*)\"");
                MatchCollection telescopeIDMatch = Regex.Matches(responseBody, "live-images/ca(.*)\"");
                string[] images = new string[6];
                string[] idImages = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    // image filename 
                    string imageName;
                    string telescopeCode;
                    imageName = imageFilesMatch[i].Value.Replace("live-images/", "").Replace("\"", "");
                    telescopeCode = telescopeIDMatch[i].Value.Replace("live-images/", "").Replace("\"", "");

                    telescopes[telescopeCode.Substring(0, 4)].TelescopePosition = imageName.Substring(0, 2);
                    telescopes[telescopeCode.Substring(0, 4)].Azimuth = azimuth;
                    telescopes[telescopeCode.Substring(0, 4)].Elevation = elevation;
                }
                // can get file from https://www.narrabri.atnf.csiro.au/public/live-images/[image_filename]
                // where image_filename is the filename
                Console.ReadLine();
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                //Console.ReadLine();
            }
        }
	}
}
