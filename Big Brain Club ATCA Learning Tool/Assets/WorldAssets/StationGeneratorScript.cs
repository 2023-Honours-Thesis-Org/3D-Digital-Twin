using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationGeneratorScript : MonoBehaviour
{

    public static float increment = 15.306f;
    public static int centrepoint = 196; // the Wdists position that will be transformed to Unity x=0
    public static int juncture = 106; // the east-west coordinate of the North arm
    public static short[] Wdists = {0, 2, 4, 6, 8, 10, 12, 14, 16, 32, 45,
                                     64, 84, 98, 100, 102, 104, 106, 109, 110, 111, 112,
                                     113, 124, 125, 128, 129, 140, 147, 148, 163, 168, 172,
                                     173, 182, 189, 190, 195, 196, 392};
    public static short[] Ndists = {2, 5, 7, 11, 14};

    public GameObject stationPrefab;
    public Dictionary<string,GameObject> stationClones;
    

    // Start is called before the first frame update
    void Start()
    {
        generateStations();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void generateStations()
    {
        foreach (short dist in Wdists) 
        {
            GameObject inst = Instantiate(stationPrefab, new Vector3((float)(centrepoint - dist) * increment, 0.0f, 0.0f), Quaternion.identity);
            inst.name = "W" + dist;

        }
        foreach (short dist in Ndists) 
        {
            GameObject inst = Instantiate(stationPrefab, new Vector3((float)(centrepoint - juncture) * increment, 0.0f, (float)dist * increment), Quaternion.identity);
            inst.name = "N" + dist;

        }
    }
    
}