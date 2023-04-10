using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{
    public CameraFollow playerCamera;
    private void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room" + Random.Range(0, 5000), new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player",
            new Vector3(
                Random.Range(-10,10),
                Random.Range(-10,10),
                1), Quaternion.identity);   
    }

}
