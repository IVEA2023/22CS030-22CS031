using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TMP_InputField _roomInput;

    [SerializeField]
    private RoomItemUI _roomItemUIPrefeb;

    [SerializeField]
    private PlayerItemUI _playerItemUIPrefeb;

    [SerializeField]
    private Transform _playerListParent;

    [SerializeField]
    private Transform _roomListParent;

    [SerializeField]
    private TMP_Text _statusFiled;

    [SerializeField]
    private Button _leaveRoomButton;

    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private Button _selectMettingButton;

    [SerializeField]
    private Button _selectGame1Button;
    
    [SerializeField]
    private Button _selectGame2Button;

    [SerializeField]
    private Button _selectLibraryButton;

    [SerializeField]
    private Button _selectShopButton;

    [SerializeField]
    public GameObject Prom;

    private List<RoomItemUI> _roomList = new List<RoomItemUI>();
    private List<PlayerItemUI> _playerList = new List<PlayerItemUI>();

    public TMP_Text Prom_text;
    private bool hasmetting = false;
    private bool hasgame1 = false;
    private bool hasgame2 = false;
    private bool haslibrary = false;
    private bool hasshop = false;

    private void Start()
    {
        Initialize();
        Connect();
    }

    private void Initialize()
    {
        _leaveRoomButton.interactable = false;
    }
    #region PhotonCallbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected!");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinedLobby!");
    }

    public override void OnJoinedRoom()
    {
        _statusFiled.text = "Joined ";
        Debug.Log("JoinedRoom! " + PhotonNetwork.CurrentRoom.Name);
        _leaveRoomButton.interactable = true;

        if (PhotonNetwork.IsMasterClient)
        {
            _startButton.interactable = true;
        }

        UpdatePlayerList();

    }

    public override void OnLeftRoom()
    {
        _statusFiled.text = "Lobby";
        Debug.Log("LeftRoom! ");
        //_leaveRoomButton.interactable = false;
        _startButton.interactable = false;
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    #endregion

    private void Connect()
    {
        PhotonNetwork.NickName = Login.GetUserName();
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomInput.text) == false)
        {
            PhotonNetwork.CreateRoom(_roomInput.text, new RoomOptions() { MaxPlayers = 4 ,BroadcastPropsChangeToAll = true});
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < _roomList.Count; i++)
        {
            Destroy(_roomList[i].gameObject);
        }

        _roomList.Clear();

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount == 0) { continue; }

            RoomItemUI newRoomItem = Instantiate(_roomItemUIPrefeb, _roomListParent);
            newRoomItem.LobbyNetworkManagerParent = this;
            Debug.Log("UpdateRoomList" + roomList[i].Name);
            newRoomItem.SetName(roomList[i].Name);
            //newRoomItem.transform.SetParent(_roomListParent);

            _roomList.Add(newRoomItem);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerList()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            Destroy(_playerList[i].gameObject);
        }

        _playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null) { return; }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItemUI newPlayerItem = Instantiate(_playerItemUIPrefeb, _playerListParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            { 
                newPlayerItem.ApplyLocalChanges();
            }

            _playerList.Add(newPlayerItem);
        }
    }

    public void OnStartClicked()
    {
        
        if (hasmetting)
        {
            Debug.Log("load into meeting room");
            hasmetting = false;
            _selectGame1Button.interactable = true;
            _selectLibraryButton.interactable = true;
            _selectGame2Button.interactable = true;
            _selectShopButton.interactable = true;
            PhotonNetwork.LoadLevel("MeetingRoom");
        }

        if (haslibrary)
        {
            Debug.Log("load into Library");
            haslibrary = false;
            _selectGame1Button.interactable = true;
            _selectGame2Button.interactable = true;
            _selectMettingButton.interactable = true;
            _selectShopButton.interactable = true;
            PhotonNetwork.LoadLevel("Library");
        }

        if (hasgame1)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount != 1)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                if (photonView.IsMine)
                {
                    photonView.RPC("promErrorMsg", RpcTarget.All, "ONLY ALLOW ONE PLAYER!");
                }
            }
            else
            {
                Debug.Log("load into game1 room");
                hasgame1 = false;
                _selectMettingButton.interactable = true;
                _selectLibraryButton.interactable = true;
                _selectGame2Button.interactable = true;
                _selectShopButton.interactable = true;
                PhotonNetwork.LoadLevel("GameRoom");
            }
        }

        if (hasgame2)
        {
            Debug.Log("load into game2 room");
            hasgame2 = false;
            _selectMettingButton.interactable = true;
            _selectLibraryButton.interactable = true;
            _selectGame1Button.interactable = true;
            _selectShopButton.interactable = true;
            PhotonNetwork.LoadLevel("TeleportGame");
        }

        if (hasshop)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount != 1)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                if (photonView.IsMine)
                {
                    photonView.RPC("promErrorMsg", RpcTarget.All, "ONLY ALLOW ONE PLAYER!");
                }
            }
            else
            {
                Debug.Log("load into shop");
                hasshop = false;
                _selectMettingButton.interactable = true;
                _selectLibraryButton.interactable = true;
                _selectGame1Button.interactable = true;
                _selectGame2Button.interactable = true;
                PhotonNetwork.LoadLevel("Shop");
            }
        }
    }

    public void setMetting() 
    {

        if (hasmetting == false)
        {
            hasmetting = true;
            _selectGame1Button.interactable = false;
            _selectGame2Button.interactable = false;
            _selectLibraryButton.interactable = false;
            _selectShopButton.interactable = false;
        }
        else
        {
            hasmetting = false;
            _selectGame1Button.interactable = true;
            _selectGame2Button.interactable = true;
            _selectLibraryButton.interactable = true;
            _selectShopButton.interactable = true;
        }

    }

    public void setLibrary()
    {

        if (haslibrary == false)
        {
            haslibrary = true;
            _selectMettingButton.interactable = false;
            _selectGame1Button.interactable = false;
            _selectGame2Button.interactable = false;
            _selectShopButton.interactable = false;
        }
        else
        {
            haslibrary = false;
            _selectMettingButton.interactable = true;
            _selectGame1Button.interactable = true;
            _selectGame2Button.interactable = true;
            _selectShopButton.interactable = true;
        }
    }

    public void setGame1()
    {

        if (hasgame1 == false)
        {
            hasgame1 = true;
            _selectMettingButton.interactable = false;
            _selectLibraryButton.interactable = false;
            _selectGame2Button.interactable = false;
            _selectShopButton.interactable = false;
        }
        else
        {
            hasgame1 = false;
            _selectMettingButton.interactable = true;
            _selectLibraryButton.interactable = true;
            _selectGame2Button.interactable = true;
            _selectShopButton.interactable = true;
        }

    }

    public void setGame2()
    {

        if (hasgame2 == false)
        {
            hasgame2 = true;
            _selectMettingButton.interactable = false;
            _selectGame1Button.interactable = false;
            _selectLibraryButton.interactable = false;
            _selectShopButton.interactable = false;
        }
        else
        {
            hasgame2 = false;
            _selectMettingButton.interactable = true;
            _selectGame1Button.interactable = true;
            _selectLibraryButton.interactable = true;
            _selectShopButton.interactable = true;
        }
    }

    public void setShop()
    {

        if (hasshop == false)
        {
            hasshop = true;
            _selectMettingButton.interactable = false;
            _selectGame1Button.interactable = false;
            _selectGame2Button.interactable = false;
            _selectLibraryButton.interactable = false;
        }
        else
        {
            hasshop = false;
            _selectMettingButton.interactable = true;
            _selectGame1Button.interactable = true;
            _selectGame2Button.interactable = true;
            _selectLibraryButton.interactable = true;
        }
    }


    [PunRPC]
    public void promErrorMsg(string msg)
    {
        Prom.SetActive(true);
        Prom_text.text = msg;
        Invoke("setPromFalse", 2.0f);
    }

    public void setPromFalse()
    {
        Prom.SetActive(false);
    }


}
