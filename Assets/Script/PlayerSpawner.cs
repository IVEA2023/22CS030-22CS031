using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrebs;
    public Vector3 spawnPoint = Vector3.zero;
 

    private void Start()
    {
        GameObject playerToSpawn = playerPrebs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatarselected"]]; 
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint, Quaternion.identity);
    }
}
