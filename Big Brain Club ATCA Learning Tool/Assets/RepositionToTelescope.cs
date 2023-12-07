using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Antenna model naming format = antennaCA0n, where n in Z and 1 <= n <= 6
// 0, 13, -32 
public class RepositionToTelescope : MonoBehaviour
{
    private string antennaName;
    [SerializeField]
    private GameObject antennaModel;
    private Vector3 offset = new Vector3(0, 13, -32);
    // Start is called before the first frame update
    void Start()
    {
        string cameraCode = (name.Split(' '))[0];
        antennaName = "antenna"+cameraCode;
        
        antennaModel = GameObject.Find(antennaName);
        gameObject.transform.parent = antennaModel.transform;
        gameObject.transform.position = antennaModel.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
