using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class GetSolution : MonoBehaviour
{
    public TMP_Text solution_gamebox1;
    public TMP_Text solution_gamebox2;
    public TMP_Text solution_gamebox3;
    public TMP_Text solution;
    public Button playagin;

    private void Start()
    {
        solution.text = solution_gamebox1.text+ "<br> " + solution_gamebox2.text + "<br>" + solution_gamebox3.text;
        playagin.interactable = true;
    }

    public void OnClickPlayAgain() 
    {
        playagin.interactable = false;
        PhotonNetwork.LoadLevel("Lobby");
    }
}
