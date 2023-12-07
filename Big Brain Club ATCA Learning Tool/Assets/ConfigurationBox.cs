using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Telescope;

public class ConfigurationBox : MonoBehaviour
{
    public AntennaController antennaController;

    public void EnableWidget()
    {
        gameObject.SetActive(true);
    }

    public void DisableWidget()
    {
        gameObject.SetActive(false);
    }

    public void setArrayConfiguration(TextMeshProUGUI button)
    {
        antennaController.setArrayConfigurationTo(button.text);
    }

}
