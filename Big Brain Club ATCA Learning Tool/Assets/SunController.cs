using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{

    public void UpdatePosition(System.DateTime dateTime)
    {
        transform.LookAt(-Coordinates.Sky2Unity(Coordinates.SunCoordinates(dateTime)));
    }
}
