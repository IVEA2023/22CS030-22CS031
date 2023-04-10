using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerItemUI : MonoBehaviourPunCallbacks
{

    [SerializeField] 
    private TMP_Text _playerName;

    //Image backgorundImage;
    public Color highlightColor;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    Hashtable playerProperties = new Hashtable();
    public GameObject playerAvatar;
    public int playerchoice;
    public GameObject[] avatars;

    private DbManager dbaccess;
    Player _player;

    private bool bought1 = false;
    private bool bought2 = false;
    private bool bought3 = false;

    private void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        getcharlist();
        playerProperties["playerAvatarselected"] = 0;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void ApplyLocalChanges()
    { 
        //backgorundImage.color = highlightColor;
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
        

    }

    public void SetPlayerInfo(Player playerName)
    {
        _playerName.text = playerName.NickName;
        _player = playerName;
        UpdatePlayerItem(_player);
    }

    public void OnClickLeftArrow()
    {
        

        if ((int)playerProperties["playerAvatarselected"] == 0)
        {
            // 0 -> 5 / 4 / 3 / 2
            if (bought3)
            {
                playerProperties["playerAvatarselected"] = avatars.Length - 1;
            }
            else if (bought2)
            {
                playerProperties["playerAvatarselected"] = avatars.Length - 2;
            }
            else if (bought1)
            {
                playerProperties["playerAvatarselected"] = avatars.Length - 3;
            }
            else
            {
                playerProperties["playerAvatarselected"] = avatars.Length - 4;
            }

        }
        else
        {

            if (((int)playerProperties["playerAvatarselected"]) == 5)
            {
                if (bought2)
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] - 1;
                }
                else
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] - 1;
                    OnClickLeftArrow();
                }
            }
            else if (((int)playerProperties["playerAvatarselected"]) == 4)
            {
                if (bought1)
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] - 1;
                }
                else
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] - 1;
                    OnClickLeftArrow();
                }
            }
            else
            {
                playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] - 1;
            }
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow() 
    {
        

        if ((int)playerProperties["playerAvatarselected"] == avatars.Length - 1)
        {
            playerProperties["playerAvatarselected"] = 0;
        }
        else
        {
            if (((int)playerProperties["playerAvatarselected"]) == 2)
            {
                if (bought1)
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
                }
                else
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
                    OnClickRightArrow();
                }
            }

            else if (((int)playerProperties["playerAvatarselected"]) == 3)
            {
                if (bought2)
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
                }
                else
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
                    OnClickRightArrow();
                }
            }

            else if (((int)playerProperties["playerAvatarselected"]) == 4)
            {
                if (bought3)
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
                }
                else
                {
                    playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
                    OnClickRightArrow();
                }
            }
            else
            {
                playerProperties["playerAvatarselected"] = (int)playerProperties["playerAvatarselected"] + 1;
            }
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (_player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {

        if (player.CustomProperties.ContainsKey("playerAvatarselected"))
        {
            
            for (int i = 0; i < avatars.Length; i++)
            {
                avatars[i].SetActive(false);
            }

            avatars[(int)player.CustomProperties["playerAvatarselected"]].SetActive(true);

            playerProperties["playerAvatarselected"] = (int)player.CustomProperties["playerAvatarselected"];

        }
        else
        {
            playerProperties["playerAvatarselected"] = 0;
        }
        
    }
    public void getcharlist()
    {
        var player_name = PhotonNetwork.NickName;
        string[] player_ctype = dbaccess.getPlayerCtypeList(player_name);
        foreach (string ctype in player_ctype)
        {
            if (ctype == "C01")
            {
                bought1 = true;
            }
            if (ctype == "C02")
            {
                bought2 = true;
            }
            if (ctype == "C03")
            {
                bought3 = true;
            }
        }
    }
}
