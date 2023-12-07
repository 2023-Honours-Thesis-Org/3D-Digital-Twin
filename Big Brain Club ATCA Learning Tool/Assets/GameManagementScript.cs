using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagementScript : MonoBehaviour
{
    [SerializeField]
    GameObject exitDialog;

    public void TurnOnConfirmationBox()
    {
        exitDialog.SetActive(true);
    }

    public void TurnOffConfirmationBox()
    {
        exitDialog.SetActive(false);
    } 

    public void ExitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
