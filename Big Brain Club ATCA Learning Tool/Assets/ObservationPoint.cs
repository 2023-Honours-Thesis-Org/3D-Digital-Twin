using System.Collections;
using System.Collections.Generic;
using Controller;
using TMPro;
using UnityEngine;

public class ObservationPoint : MonoBehaviour
{
    public Camera obsCamera;
    public TextMeshProUGUI alpha;
    public TextMeshProUGUI delta;

    public TelescopeController telescopeController;

    public Vector3 direction;
    public Vector3 coordinates; // Sky coords.
    public Vector2 Equitorial; // Equitorial (rA, Dec).
    
    public ObservationManager obsMan;

    // Start is called before the first frame update
    void Start()
    {
        // Find where its pointing at in the sky which is 3500 units away
        direction = 3500.0f * obsCamera.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        direction = 3500.0f * obsCamera.transform.forward;
        coordinates = Coordinates.Unity2Sky(direction);
        Equitorial = Coordinates.Sky2EquatorialSky(coordinates, true);
        
        alpha.text = "" + Equitorial.x;
        delta.text = "" + Equitorial.y;

        // Debug.Log(coordinates);
    }

    public void SendObservationPoint(TMP_InputField input) 
    {
        float freq = float.Parse(input.text);
        // telescopeController.SetTelescopeSkyCoordinates(coordinates);
        obsMan.ConfigOptions(freq, (float) Equitorial.y);
        obsMan.FinalImage();
    }

    public void RegisterStarPoint()
    {
        obsMan.obsQueue.RegisterStartPoint(coordinates);
    }
}
