using UnityEngine;
using TMPro;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
public class ShowQuiz : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView myPY;


    private DbManager dbaccess;

    public GameObject question_obj;
    public GameObject ans1_obj;
    public GameObject ans2_obj;
    public GameObject ans3_obj;


    public BoxCollider door1;
    public BoxCollider door2;
    public BoxCollider door3;

    public TMP_Text solution;

    TextMeshProUGUI question_text;
    TextMeshProUGUI ans1_text;
    TextMeshProUGUI ans2_text;
    TextMeshProUGUI ans3_text;
    
    

    string quiz = "";
    string correctans = "";
    string wrongans1 = "";
    string wrongans2 = "";

    // Start is called before the first frame update
    void Start()
    {

        dbaccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DbManager>();
        

        door1 = door1.GetComponent<BoxCollider>();
        door2 = door2.GetComponent<BoxCollider>();
        door3 = door3.GetComponent<BoxCollider>();

        question_text = question_obj.GetComponent<TextMeshProUGUI>();
        ans1_text = ans1_obj.GetComponent<TextMeshProUGUI>();
        ans2_text = ans2_obj.GetComponent<TextMeshProUGUI>();
        ans3_text = ans3_obj.GetComponent<TextMeshProUGUI>();


        if (PhotonNetwork.IsMasterClient)
        {
            Getquestions();

        }

        myPY.RPC("DisplayQuestion", RpcTarget.All);

    }


    void doorbreak(BoxCollider door)
    {
        //turn collision off for correct answer
        door.size = new Vector3 (0, 0, 0); 
    }


    public void Getquestions()
    {
        var question = dbaccess.GetQuestionsFromDB();

        System.Random r = new System.Random();
        int n = r.Next(3);

        string[] subs = question[n].Split(',');

        quiz = subs[0].ToString();

        var sequence = Enumerable.Range(1, 3).OrderBy(n => n * n * (new System.Random()).Next());
        var result = sequence.Distinct().Take(3);

        int[] alldoor = new int[] { };
        foreach (int num in result)
        {
            alldoor = alldoor.Concat(new int[] { num }).ToArray();
        }

        correctans = subs[alldoor[0]].ToString();
        wrongans1 = subs[alldoor[1]].ToString();
        wrongans2 = subs[alldoor[2]].ToString();

        if (alldoor[0] == 1)
        {
            doorbreak(door1);
        }
        if (alldoor[1] == 1)
        {
            doorbreak(door2);
        }
        if (alldoor[2] == 1)
        {
            doorbreak(door3);
        }

    }



    [PunRPC]
    public void DisplayQuestion()
    {
        ans1_text.text = correctans;
        ans2_text.text = wrongans1;
        ans3_text.text = wrongans2;
        question_text.text = quiz;
    }

}

