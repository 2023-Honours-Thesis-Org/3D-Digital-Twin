using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerClickHandler
{
    public TabGroup tabGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.TabSelected(this);
        Debug.Log("Clicked This Tab!!!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
