using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text playername;
    void Start()
    {
        //string spwanplayername = PhotonNetwork.LocalPlayer.CustomProperties["username"].ToString();
        //Debug.Log("Player?NAME: " + spwanplayername);
        //playername.text = spwanplayername;
        playername.text = photonView.Owner.NickName;
    }
}
