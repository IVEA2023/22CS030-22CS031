using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject manuel_detail;

    

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {

            menuPanel.SetActive(!menuPanel.activeSelf);
        }
        
    }
}
