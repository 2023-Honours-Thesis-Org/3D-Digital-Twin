using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Source:
 * https://www.youtube.com/watch?v=sD0vNXQYY_U&list=PLzkFg5FdGsozXHpdiazJLTu8frgCobL-7&index=8&ab_channel=DALAB
 */

public class CameraControl : MonoBehaviour
{
    public float roationSpeed = 500.0f;
    private Vector3 mouseWorldPosStart;
    public float zoomScale = 5.0f;
    public float zoomMin = 0.1f;
    public float zoomMax = 400.0f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) &&
            Input.GetKey(KeyCode.Mouse1))
        {
            CanOrbitVertical();
        }

        if (Input.GetKey(KeyCode.LeftShift) &&
            Input.GetKey(KeyCode.Mouse0))
        {
            CanOrbitHorizontal();
        }

        if (Input.GetMouseButtonDown(2) &&
            Input.GetKey(KeyCode.LeftControl))
        {
            mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2) &&
            Input.GetKey(KeyCode.LeftControl))
        {
            Pan();
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void CanOrbitHorizontal()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            float horizontalInput = Input.GetAxis("Mouse X") * roationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, -horizontalInput);
        }
    }
    private void CanOrbitVertical()
    {
        if (Input.GetAxis("Mouse Y") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * roationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
        }
    }

    private void Pan()
    { 
        if (Input.GetAxis("Mouse Y") != 0 ||
            Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main
                                                                   .ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff;
        }
    }

    private void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomDiff * zoomScale,
                                                        zoomMin, zoomMax);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main
                                                                    .ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff;
        }
    }
}
