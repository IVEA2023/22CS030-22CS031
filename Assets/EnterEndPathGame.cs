using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterEndPathGame : MonoBehaviourPunCallbacks
{
    public GameObject endgame;
    public GameObject GUI;

    private DbManager dbaccess;

    private void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player_name = PhotonNetwork.NickName;

        dbaccess.updateUserCurrencyData(player_name, 10);

        endgame.SetActive(true);
        GUI.SetActive(false);
    }
}
