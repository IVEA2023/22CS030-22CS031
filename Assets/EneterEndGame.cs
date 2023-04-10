using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class EneterEndGame : MonoBehaviourPunCallbacks
{

    public TMP_Text winboard;
    public GameObject Prom;
    public GameObject Prom2;
    public GameObject Prom3;

    public TMP_Text Prom_text;    
    public TMP_Text Prom_text2;
    public TMP_Text Prom_text3;

    private DbManager dbaccess;

    public TMP_Text solution_1;
    public TMP_Text solution_2;
    public TMP_Text solution_3;
    public TMP_Text solution_4;
    public TMP_Text solution_5;
    public TMP_Text solution_6;

    private List<string> scoreboard = new List<string>();

    private void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = PhotonNetwork.NickName;
            foreach (string n in scoreboard)
            {
                if (n == player)
                {
                    return;
                }
            }

            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("UpdateScoreBoard", RpcTarget.All, player);
        }

    }


    [PunRPC]
    public void UpdateScoreBoard(string player) 
    {
        scoreboard.Add(player);
        UpdateRankBoard();
    }

    public void UpdateRankBoard()
    {
        int count = 0;
        winboard.text = "";
        foreach (string n in scoreboard)
        {
            count += 1;
            winboard.text += count.ToString();
            winboard.text += ". ";
            winboard.text += n;
            winboard.text += "<br>";
            
        }

        if (count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Engame();
        }
            
    }


    public void Engame() 
    {

        int reward = 40;
        foreach (string name in scoreboard)
        {
            dbaccess.updateUserCurrencyData(name, reward);
            reward -= 10;
        }


        Prom.SetActive(true);
        Prom_text.text = "All of you had done the great job!";
        Invoke("setPromFalse", 5.0f);

        Prom2.SetActive(true);
        Prom_text2.text = "Reward had sent to your currency!";
        Invoke("setPromFalse", 5.0f);

        Invoke("showAnswer", 5.0f);
        
    }

    public void setPromFalse()
    {
        Prom.SetActive(false);
        Prom2.SetActive(false);
    }

    public void showAnswer() 
    {
        Prom3.SetActive(true);
        string line = "!!!SOLUTION!!!"+"<br>" + "<br>" + solution_1.text + "<br>" + "<br>" + solution_2.text + "<br>" + "<br>" + solution_3.text + "<br> " + "<br>" + solution_4.text +  "<br>" + "<br>" + solution_5.text +  "<br>" + "<br>" + solution_6.text;
        Prom_text3.text = line;
        Invoke("setAnswerPromFalse", 10.0f);
    }

    public void setAnswerPromFalse()
    {
        Prom3.SetActive(false);
    }


}
