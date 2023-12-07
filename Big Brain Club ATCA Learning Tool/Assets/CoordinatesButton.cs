using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Controller;

public class CoordinatesButton : MonoBehaviour
{
    public TMP_InputField alphaInput;
    public TMP_InputField deltaInput;
    public TMP_InputField distInput;
    public TelescopeController telescopeController;
    public GameObject observedImage;

    public void pointAtCoordinates()
    {
        float alphaFloat = float.Parse(alphaInput.text);
        float deltaFloat = float.Parse(deltaInput.text);
        float distFloat = float.Parse(distInput.text) * 1000;
        Vector2 equatorialSkyCoords = new Vector2(alphaFloat, deltaFloat);
        Vector3 skyCoords = Coordinates.EquatorialSky2Sky(equatorialSkyCoords, distFloat, true);
        telescopeController.SetTelescopeSkyCoordinates(skyCoords);
    
        Vector3 unityCoords = Coordinates.Sky2Unity(skyCoords);
        unityCoords.x = (int) unityCoords.x;
        unityCoords.y = (int) unityCoords.y;
        unityCoords.z = (int) unityCoords.z;


        observedImage.transform.LookAt(unityCoords);
    }
}
