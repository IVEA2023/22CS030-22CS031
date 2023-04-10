using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetInternalName : MonoBehaviour
{

    TextMeshProUGUI userText;


    [SerializeField]
    private GameObject playername;


    public string Getname()
    {
        userText = playername.GetComponent<TextMeshProUGUI>();
        return userText.text;
    }

}
