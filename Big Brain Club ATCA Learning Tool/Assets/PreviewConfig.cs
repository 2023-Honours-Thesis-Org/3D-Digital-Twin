using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PreviewConfig : MonoBehaviour
{
    public RawImage configImage;
    private string selected;
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI current;
    // Start is called before the first frame update
    void Start()
    {
        selected = current.text;
        foreach (TMP_Dropdown.OptionData option in dropdown.options)
        {
            if (selected == option.text)
            {
                configImage.texture = option.image.texture;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != current.text)
        {
            selected = current.text;
            foreach (TMP_Dropdown.OptionData option in dropdown.options)
            {
                if (selected == option.text)
                {
                    configImage.texture = option.image.texture;
                }
            }
        }
    }
}
