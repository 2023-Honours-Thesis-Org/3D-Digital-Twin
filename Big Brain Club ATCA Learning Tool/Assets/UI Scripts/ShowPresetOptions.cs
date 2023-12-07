using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPresetOptions : MonoBehaviour
{
    private bool toggle = false;

    [SerializeField] public GameObject presetWindow;
    // Start is called before the first frame update
    void TogglePresetWindow()
    {
        toggle = !toggle;
        presetWindow.SetActive(toggle);
    }
}
