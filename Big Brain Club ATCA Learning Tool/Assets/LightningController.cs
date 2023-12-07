using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    public GameObject lightningOne;
    public GameObject lightningTwo;
    public GameObject lightningThree;
    public bool lightingEnabled = false;
    double waitTime = 0.0;

    //public GameObject audio;
    // Start is called before the first frame update
    void Start()
    {
        lightningOne.SetActive(false);
        lightningTwo.SetActive(false);
        lightningThree.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (lightingEnabled)
        {
            waitTime -= UnityEngine.Time.deltaTime;
            int r = Random.Range(10, 30);
            if (waitTime <= 0)
            {
                Invoke("makeItShine", r);
                waitTime = r;
            }
        }
    }

    void makeItShine()
    {
        int r = Random.Range(0, 3);
        if (r == 0)
        {
            lightningOne.SetActive(true);
            Invoke("stopTheShine", 0.125f);
        } else if (r == 1)
        {
            lightningTwo.SetActive(true);
            Invoke("stopTheShine", 0.105f);

        } else
        {
            lightningThree.SetActive(true);
            Invoke("stopTheShine", 0.75f);
        }
    }

    void stopTheShine()
    {
        lightningOne.SetActive(false);
        lightningTwo.SetActive(false);
        lightningThree.SetActive(false);
    }

    void boom()
    {

    }

    void StopBoom()
    {

    }
}
