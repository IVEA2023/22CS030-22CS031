using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TeleopgameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Vector3 telport_position_correct;

    [SerializeField]
    private Vector3 telport_position_wrong;

    private DbManager dbaccess;

    public TMP_Text question_tmp;
    public TMP_Text ans1_tmp;
    public TMP_Text ans2_tmp;
    public TMP_Text ans3_tmp;
    public TMP_Text solution_tmp;

    public GameObject door_tel_1;
    public GameObject door_tel_2;
    public GameObject door_tel_3;

    string realcorrect = "";
    string quiz = "";
    string correctans = "";
    string wrongans1 = "";
    string wrongans2 = "";
    int[] alldoor = new int[] { };

    void Start()
    {
        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();

        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            Getquestions();
            photonView.RPC("DisplayQuestion", RpcTarget.All, quiz, correctans, wrongans1, wrongans2, alldoor, realcorrect);
        }
    }


    public void Getquestions()
    {
        var question = dbaccess.GetQuestionsFromDB();
        Debug.Log("[Show] | " + question);

        System.Random r = new System.Random();
        System.Random r1 = new System.Random();
        System.Random r2= new System.Random();
        int n1 = r.Next(50);        
        int n2 = r1.Next(25);
        int n3 = r2.Next(25);

        int n = n1 + n2 + n3;
        string[] subs = question[n].Split(',');

        quiz = subs[0].ToString();
        realcorrect = subs[1].ToString();

        var sequence = Enumerable.Range(1, 3).OrderBy(n => n * n * (new System.Random()).Next());
        var result = sequence.Distinct().Take(3);

        
        foreach (int num in result)
        {
            alldoor = alldoor.Concat(new int[] { num }).ToArray();
        }

        correctans = subs[alldoor[0]].ToString();
        wrongans1 = subs[alldoor[1]].ToString();
        wrongans2 = subs[alldoor[2]].ToString();

    }



    [PunRPC]
    public void DisplayQuestion(string q, string c, string w1, string w2, int[] ad, string rc)
    {
        UpdateUI(q, c, w1, w2, ad, rc);
    }

    public void UpdateUI(string q, string c, string w1, string w2, int[] ad, string rc)
    {
        question_tmp.text = q;
        ans1_tmp.text = c;
        ans2_tmp.text = w1;
        ans3_tmp.text = w2;
        solution_tmp.text = ("Q: " + q + "<br>" +"A: " + rc);

        if (ad[0] == 1)
        {
            door_tel_1.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_correct);
            door_tel_2.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_wrong);
            door_tel_3.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_wrong);
        }
        if (ad[1] == 1)
        {
            door_tel_1.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_wrong);
            door_tel_2.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_correct);
            door_tel_3.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_wrong);
        }
        if (ad[2] == 1)
        {
            door_tel_1.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_wrong);
            door_tel_2.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_wrong);
            door_tel_3.GetComponent<Teloport_trigger>().setTelportPoint(telport_position_correct);
        }
    }
}
