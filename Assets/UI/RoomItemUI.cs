using UnityEngine;
using TMPro;

public class RoomItemUI : MonoBehaviour
{
    public LobbyNetworkManager LobbyNetworkManagerParent;

    [SerializeField] 
    private TMP_Text _roomName;



    public void SetName(string roomName)
    { 
        _roomName.text = roomName;
    }

    public void OnJoinPressed()
    {
        Debug.Log("RoomItemUI"+_roomName.text);
        LobbyNetworkManagerParent.JoinRoom(_roomName.text);
    }

}
