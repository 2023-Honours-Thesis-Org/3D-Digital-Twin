using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float movementTime;
    [SerializeField]
    private float rotationAmount;

    [SerializeField]
    private Vector3 newPosition;
    private Vector3 homePosition;
    [SerializeField]
    private Quaternion newRotation;
    private Quaternion homeRotation;
    [SerializeField]
    private Vector3 newXRotation;
    private Vector3 homeXRotation;

    [SerializeField]
    private bool useEdgeScrolling;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField]
    private float maxZoom = 100f;
    [SerializeField]
    private float minZoom = 50f;

    [SerializeField]
    private Vector3 dragStartPosition;
    [SerializeField]
    private Vector3 dragCurrentPosition;

    private float targetZoom;

    private CinemachineComposer composer;

    void Start()
    {
        newPosition = transform.position;
        homePosition = transform.position;
        newRotation = transform.rotation;
        homeRotation = transform.rotation;
        useEdgeScrolling = false;
        composer = (CinemachineComposer) cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
        newXRotation = composer.m_TrackedObjectOffset;
        homeXRotation = composer.m_TrackedObjectOffset;
        
    }


    private void Update()
    {
        HandleCameraZoom();
        HandleMovementInput();
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        composer.m_TrackedObjectOffset = Vector3.Lerp(composer.m_TrackedObjectOffset, newXRotation, Time.deltaTime * movementTime);
    }

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            newXRotation += (Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newXRotation += (Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.H))
        {
            newRotation = homeRotation;
            newXRotation = homeXRotation;
        }

        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.H))
        {
            newPosition = homePosition;
        }
        
    }
    

    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            // to change zoom
            targetZoom -= 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            // to change zoom
            targetZoom += 5;
        }

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        cinemachineVirtualCamera.m_Lens.FieldOfView = targetZoom;
    }
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition  = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition  = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }
    
}