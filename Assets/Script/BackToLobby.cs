using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BackToLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button bktoLobby;

    [SerializeField]
    private Button manual;

    [SerializeField]
    private GameObject manual_detail;

    [SerializeField]
    private TMP_Text currency;

    private DbManager dbaccess;
    public void BackToLobby_Room()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LoadLevel("Lobby");

    }

    public void OnClickManual()
    {
        if (manual_detail.activeSelf)
        {
            manual_detail.SetActive(false);
        }
        else
        {
            manual_detail.SetActive(true);
        }
        
    }

    void Start()
    {
        Invoke("updateuserinfo", 1.0f);
    }

    public void updateuserinfo()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        currency.text = "Point: " + dbaccess.getPlayerCurrency(PhotonNetwork.NickName).ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
            currency.text = "Point: " + dbaccess.getPlayerCurrency(PhotonNetwork.NickName).ToString();
        }
    }

}
