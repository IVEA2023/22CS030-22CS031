using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Onclose : MonoBehaviour
{
    private DbManager dbaccess;
    void OnApplicationQuit()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        Debug.Log("Application ending after " + Time.time + " seconds");
        var name = PhotonNetwork.NickName.ToString().Trim();
        Debug.Log(name);
        dbaccess.updateLogin(name, "1");
    }
}