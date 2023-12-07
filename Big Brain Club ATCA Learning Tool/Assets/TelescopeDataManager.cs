using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using UnityEngine.Networking;
using System;
using System.Text;

namespace NetworkOperations
{
    public class TelescopeDataManager : RequestHandler
    {

        // private const string DATA_URI = "https://www.narrabri.atnf.csiro.au/cgi-bin/Public/atca_live.cgi";
        private const string DATA_URI = "https://www.narrabri.atnf.csiro.au/cgi-bin/obstools/web_monica/monicainterface_json_atca_status.pl";

        public TelescopeController telescopeController;
        private bool realTimeEnabled = true;

        private float timer = 0f;
        private float startTime = 0f;
        private float updateInterval = 10f;

        private void TurnOff()
        {
            realTimeEnabled = false;
        }

        private void TurnOn()
        {
            realTimeEnabled = true;
        }

        public IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch(webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        byte[] data = webRequest.downloadHandler.data;
                        string res = Encoding.Default.GetString(data);
                        Result = res;
                        break;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Result = "";
            StartCoroutine(GetRequest(DATA_URI));
            EventManager.RealTimeEvent += TurnOn;
            EventManager.UserTimeEvent += TurnOff;
        }
        
        

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            float seconds = timer % 100;
            if ((seconds - startTime) >= updateInterval)
            {
                switch (realTimeEnabled)
                {
                    case true:
                        StartCoroutine(GetRequest(DATA_URI));

                        telescopeController.SetTelescopeData(Result);

                        break;
                }
                startTime = seconds;
            }
        }

        private void OnDisable()
        {
            EventManager.RealTimeEvent -= TurnOff;
            EventManager.UserTimeEvent -= TurnOn;
        }
    }
}