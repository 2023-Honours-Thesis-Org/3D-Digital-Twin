using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetTab : Tab
{
    public Camera obsCamera;
    public Camera mainCamera;

    public bool obsCameraActive;
    public new void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.TabSelected(this);
        obsCamera.enabled = !obsCamera.enabled;
        mainCamera.enabled = !mainCamera.enabled;
        Debug.Log("Clicked This Tab!!!");
    }
    // Start is called before the first frame update
    void Start()
    {
        obsCameraActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
