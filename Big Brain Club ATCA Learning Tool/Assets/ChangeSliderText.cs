using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeSliderText : MonoBehaviour
{
    public TextMeshProUGUI valueLabel;
    public Slider valueSlider;
    // Start is called before the first frame update
    void Start()
    {
        valueLabel.text = valueSlider.value.ToString("0.0");
    }

    // Update is called once per frame
    void Update()
    {
        valueLabel.text = valueSlider.value.ToString("0.0");
    }
}
