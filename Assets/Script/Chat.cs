using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Chat : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject Message;
    public GameObject Content;
    public Button sendContentButton;

    public void SendMessage()
    {
        if (inputField.text == null || inputField.text.Trim().Equals(""))
        {
            inputField.text = "";
            return;
        }
        else
        {
            GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, (PhotonNetwork.NickName + " : " + inputField.text));
        }
        inputField.text = "";
    }


    [PunRPC]
    public void GetMessage(string ReceiveMessage)
    {
        GameObject M = Instantiate(Message, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<Message>().MyMessage.text = ReceiveMessage;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter)) 
        {
            sendContentButton.onClick.Invoke();
        }
    }
}
