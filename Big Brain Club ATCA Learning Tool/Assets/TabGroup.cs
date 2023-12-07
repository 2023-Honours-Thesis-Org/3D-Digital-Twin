using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{  
    public List<Tab> tabs = new();
    public List<GameObject> pages = new();

    public Camera obsCamera;
    public Camera mainCamera;

    public ArrayConfigData data;
    public GameTime gameTime;

    public GameObject crossHair;

    public void TabSelected(Tab tab)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == tabs.IndexOf(tab))
            {
                pages[i].SetActive(!pages[i].active);
                if (pages[i].name == "TargetSelectionPage") {
                    obsCamera.enabled = pages[i].active;
                    mainCamera.enabled = !pages[i].active;
                    GameObject.Find("GameTime").GetComponent<GameTime>().stopTime = pages[i].active;

                    gameTime.HAtoTime(data.firstHAStart);
                    crossHair.SetActive(pages[i].active);
                }
            } 
            else
            {
                pages[i].SetActive(false);
                if (pages[i].name == "TargetSelectionPage") {
                    obsCamera.enabled = false;
                    mainCamera.enabled = true;
                    GameObject.Find("GameTime").GetComponent<GameTime>().stopTime = false;
                    
                    gameTime.HAtoTime(data.firstHAStart);
                    crossHair.SetActive(false);
                }
            }
        }
    }
}
