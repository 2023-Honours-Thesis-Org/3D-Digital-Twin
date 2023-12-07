using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamera;
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(mainCamera.position.x, transform.position.y, mainCamera.position.z);

        // If we want roation
        // Vector3 newRotation = new Vector3(90, mainCamera.eulerAngles.y, 0);

        // transform.rotation = Quaternion.Euler(newRotation);
    }
}
