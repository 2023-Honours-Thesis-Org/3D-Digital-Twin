using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RainController : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem rain;
    Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);

    void Start()
    {
        rain = GetComponent<ParticleSystem>();
        var e = rain.emission;
        e.rateOverTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Transform>().rotation = Quaternion.Euler(rotation);
        //GetComponent<Transform>().localRotation = Quaternion.Euler(rotation);
    }

    public void setRainSpeed(float newRainSpeed)
    {
        var e = rain.emission;
        e.rateOverTime = newRainSpeed;
    }
}
