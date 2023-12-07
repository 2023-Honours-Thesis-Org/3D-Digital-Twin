using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCamera
{
    public class CameraController : MonoBehaviour
    {
        private CameraInputActions cameraActions;
        private InputAction movement;
        private Transform cameraTransform;


        [SerializeField]
        private Camera gameCamera;
        // Horizontal motion
        [SerializeField]
        private float maxSpeed = 5f;
        private float speed;
        [SerializeField]
        private float acceleration = 10f;
        [SerializeField]
        private float damping = 15f;

        // Vertical motion - zooming
        [SerializeField]
        private float stepSize = 2f;
        [SerializeField]
        private float zoomDampening = 7.5f;
        [SerializeField]
        private float minHeight = 5f;
        [SerializeField]
        private float maxHeight = 50f;
        [SerializeField]
        private float zoomSpeed = 2f;

        // Rotation
        [SerializeField]
        private float maxRotationSpeed = 1f;

        // Screen edge motion
        [SerializeField]
        [Range(0f, 0.1f)]
        private float edgeTolerance = 0.05f;
        [SerializeField]
        private bool useScreenEdge = true;

        // Value set in various functions
        // Used to update the position of the camera base object
        private Vector3 targetPosition;

        private float zoomHeight;

        // Used to track tand maintaine velecotu w/o a rigid body
        private Vector3 horizontalVelocity;
        [SerializeField]
        private Vector3 lastPosition;

        // Tracks where the dragging action started
        Vector3 startDrag;

        private void Update()
        {
            GetKeyboardMovement();

            UpdateVelocity();
            UpdateCameraPosition();
            UpdateBasePosition();
        }

        private void Awake()
        {
            cameraActions = new CameraInputActions();
            cameraTransform = gameCamera.transform;
        }

        private void OnEnable()
        {
            zoomHeight = cameraTransform.localPosition.y;
            // cameraTransform.LookAt(this.transform);

            lastPosition = this.transform.position;
            movement = cameraActions.Camera.Movement;
            cameraActions.Camera.RotateCamera.performed += RotateCamera;
            cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
            cameraActions.Camera.Enable();
        }

        private void OnDisable()
        {
            cameraActions.Camera.RotateCamera.performed -= RotateCamera;
            cameraActions.Disable();
        }

        private void UpdateVelocity()
        {
            horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
            horizontalVelocity.y = 0;
            lastPosition = this.transform.position;
        }

        private void GetKeyboardMovement()
        {
            Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() +
                                 movement.ReadValue<Vector2>().y * GetCameraForward();
            
            inputValue = inputValue.normalized;
            
            if (inputValue.sqrMagnitude > 0.1f)
            {
                targetPosition += inputValue;
            }
        }

        private Vector3 GetCameraRight()
        {
            Vector3 right = cameraTransform.right;
            right.y = 0;
            return right;
        }

        private Vector3 GetCameraForward()
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            return forward;
        }

        private void UpdateBasePosition()
        {
            if (targetPosition.sqrMagnitude > 0.1f)
            {
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
                cameraTransform.position += targetPosition * speed * Time.deltaTime;
            } 
            else
            {
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
                transform.position += horizontalVelocity * Time.deltaTime;
            }

            targetPosition = Vector3.zero;
        }

        private void RotateCamera(InputAction.CallbackContext inputValue)
        {
            if (!Mouse.current.middleButton.isPressed)
            {
                return;
            }

            float value = inputValue.ReadValue<Vector2>().x;
            transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
        }

        private void ZoomCamera(InputAction.CallbackContext inputValue)
        {
            float value = -inputValue.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(value) > 0.1f)
            {
                zoomHeight = cameraTransform.position.z + value * stepSize;
                if (zoomHeight < minHeight)
                {
                    zoomHeight = minHeight;
                }
                else if (zoomHeight > maxHeight)
                {
                    zoomHeight = maxHeight;
                }
            }
        }

        private void UpdateCameraPosition()
        {
            Vector3 zoomTarget = new Vector3(cameraTransform.position.x, zoomHeight, cameraTransform.position.z);
            zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.position.y) * Vector3.forward;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, zoomTarget, Time.deltaTime * zoomDampening);
        }
    }
}