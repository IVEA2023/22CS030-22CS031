using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Content : MonoBehaviour
{
    public TMP_Text ContentText;
    // Start is called before the first frame update
    public void setContentText(string text)
    {
        ContentText.text = text;
    }

    public string getContentText()
    {
        return ContentText.text;
    }
}
