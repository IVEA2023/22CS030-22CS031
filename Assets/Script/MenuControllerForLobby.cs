using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerForLobby : MonoBehaviour
{
    public GameObject menuPanel;

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }

    }
}
