using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkOperations
{
    public abstract class RequestHandler : MonoBehaviour
    {
        private string _result;
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            Result = "";
        }

        protected IEnumerator GetRequest(string uri)
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
                            Result = webRequest.downloadHandler.text;
                            break;
                    }
                }
            }
    }
}