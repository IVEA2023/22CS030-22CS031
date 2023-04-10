using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public TMP_Text MyMessage;
    // Start is called before the first frame update
    void Start()
    {
        if(MyMessage == null || MyMessage.Equals(""))
        {
            return;
        }
        GetComponent<RectTransform>().SetAsFirstSibling();
    }

    public void clickable()
    {
        var content = MyMessage.text.Split(": ");

        content[1].ToString().CopyToClipboard();
    }

}
