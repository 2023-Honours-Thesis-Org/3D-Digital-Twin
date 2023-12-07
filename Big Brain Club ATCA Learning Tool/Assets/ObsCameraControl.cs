using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsCameraControl : MonoBehaviour
{
    public Camera camera;
    // Update is called once per frame
    void Update()
    {
        if (camera.enabled) 
        {
            camera.tag = "MainCamera";
        
            if (camera.transform.rotation.x > 0)
            {
                Quaternion currentPost = camera.transform.rotation;
                camera.transform.rotation = new Quaternion(0, currentPost.y,
                                                        0, currentPost.w);
            }
        }
    }

    private void FixedUpdate()
    {
        
        if (camera.enabled) 
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Camera.main.transform.RotateAround(Camera.main.transform.position, 
                                            Camera.main.transform.right,
                                            Input.GetAxis("Mouse Y"));
                
                Camera.main.transform.RotateAround(Camera.main.transform.position, 
                                            Vector3.up,
                                            -Input.GetAxis("Mouse X"));
            }
        }

        return;
    } 
}
