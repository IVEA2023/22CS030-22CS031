using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenuContent : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button logout_lobby;

    [SerializeField]
    private Button quitgame_lobby;

    [SerializeField]
    private TMP_Text username;

    [SerializeField]
    private TMP_Text currency;

    private DbManager dbaccess;
    public void OnClickLogOut()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        var name = PhotonNetwork.NickName.ToString().Trim();
        Debug.Log(name);
        dbaccess.updateLogin(name, "1");

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Login");
    }

    public void OnClickQuitGame()
    { 
        Application.Quit();
        //EditorApplication.isPlaying = false;
    }

    void Start()
    {
        Invoke("updateuserinfo", 1.0f);
    }

    public void updateuserinfo()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        username.text = PhotonNetwork.NickName;
        currency.text = "Point: " + dbaccess.getPlayerCurrency(username.text.ToString()).ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
            currency.text = "Point: " + dbaccess.getPlayerCurrency(PhotonNetwork.NickName).ToString();
        }
    }
}
