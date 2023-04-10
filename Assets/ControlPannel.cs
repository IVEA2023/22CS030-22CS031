using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;
using System.Xml.Serialization;
using Unity.VisualScripting;

public class ControlPannel : MonoBehaviour
{
    public Button WalkButton;
    public Button PickUpButton;
    public Button TurnLeftButton;
    public Button TurnRightButton;
    public Button CancelButton;
    public Button SubmitButton;

    public TMP_Text Display_ControlStep;

    public GameObject EndGame;
    public GameObject GUI;

    public GameObject MovementBlcok_preb;
    public GameObject MovementBlcok;

    public Vector3 spawnpoint = Vector3.zero;

    public bool facingLeft;
    public bool facingRight;
    public bool facingFront;
    public bool facingBack;

    public bool hasSubmit = false;

    private float newPositionX;
    private float newPositionZ;
    private int newRotationY;
    public int intialRotationY = 0;
   
    private List<string> path = new List<string>();

    public GameObject[] gameRoom;

    public void Start()
    {
        System.Random r = new System.Random();
        int n = r.Next(1,5);

        gameRoom[n].SetActive(true);


        Initiate();
    }

    public void Initiate()
    {
        MovementBlcok = (GameObject)Instantiate(MovementBlcok_preb, spawnpoint, Quaternion.identity);
        EndGame.SetActive(false);
        GUI.SetActive(true);
        path.Clear();
        showPath();
        facingLeft = true;
        facingRight = false;
        facingFront = false;
        facingBack = false;
    }

    public void OnClickWalk()
    {
        path.Add("Walk");
        showPath();
    }

    public void OnClickTurnLeft()
    {
        path.Add("Turnleft");
        showPath();
    }

    public void OnClickTurnRight()
    {
        path.Add("TurnRight");
        showPath();
    }

    public void Walk()
    {
        //walk leftward
        if (facingLeft)
        {
            newPositionX = (float)MovementBlcok.transform.position.x - 2;
            WalkMovementX(newPositionX);
        }
        if (facingRight)
        {
            newPositionX = (float)MovementBlcok.transform.position.x + 2;
            WalkMovementX(newPositionX);
        }
        if (facingFront)
        {
            newPositionZ = (float)MovementBlcok.transform.position.z + 2;
            WalkMovementZ(newPositionZ);
        }
        if (facingBack)
        {
            newPositionZ = (float)MovementBlcok.transform.position.z - 2;
            WalkMovementZ(newPositionZ);
        }
    }

    public void TurnLeft()
    {

        //  0 -> -90 -> -180 -> -270 -> 0
        if ((intialRotationY == -270))
        {
            intialRotationY = 0;
            newRotationY = 0;
            RotationMovement(newRotationY);
        }
        else
        {
            intialRotationY -= 90;
            newRotationY = intialRotationY;
            RotationMovement(newRotationY);
        }
        CheckFacing(newRotationY);

    }

    public void TurnRight()
    {
        // 0 -> 90 -> 180 -> 270 -> 0
        if ((intialRotationY == 270))
        {
            intialRotationY = 0;
            newRotationY = 0;
            RotationMovement(newRotationY);
        }
        else
        {
            intialRotationY += 90;
            newRotationY = intialRotationY;
            RotationMovement(newRotationY);
        }
        CheckFacing(newRotationY);
    }

    public void CheckFacing(int degree)
    {
        if (degree == 0)
        {
            facingLeft = true;
            facingRight = false;
            facingFront = false;
            facingBack = false;
        }

        if (degree == 90 || degree == -270)
        {
            facingLeft = false;
            facingRight = false;
            facingFront = true;
            facingBack = false;
        }

        if (degree == 180 || degree == -180)
        {
            facingLeft = false;
            facingRight = true;
            facingFront = false;
            facingBack = false;
        }

        if (degree == 270 || degree == -90)
        {
            facingLeft = false;
            facingRight = false;
            facingFront = false;
            facingBack = true;
        }
    }

    public void showPath()
    {
        string line = "";
        if (path.Count != 0)
        {
            foreach (string p in path)
            {
                line += p;
                line += " > ";

            }
            line += "...";

            Display_ControlStep.text = line;
        }
        else 
        {
            Display_ControlStep.text = line;
        }
    }

    public void WalkMovementX(float px)
    {
        float current_X = px;
        float current_Y = 2f;
        float current_Z = MovementBlcok.transform.position.z;
        Vector3 newposition = new Vector3(current_X, current_Y, current_Z);
        MovementBlcok.transform.position = newposition;
    }

    public void WalkMovementZ(float pZ)
    {
        float current_X = MovementBlcok.transform.position.x;
        float current_Y = 2f;
        float current_Z = pZ;
        Vector3 newposition = new Vector3(current_X, current_Y, current_Z);
        MovementBlcok.transform.position = newposition;
    }

    public void RotationMovement(int ry)
    {
        float current_X = MovementBlcok.transform.position.x;
        float current_Y = 3f;
        float current_Z = MovementBlcok.transform.position.z;
        Vector3 newposition = new Vector3(current_X, current_Y, current_Z);
        MovementBlcok.transform.position = newposition;


        int current_X_R = 0;
        int current_Y_R = ry;
        int current_Z_R = 0;
        Vector3 newrotation = new Vector3(current_X_R, current_Y_R, current_Z_R);
        MovementBlcok.transform.eulerAngles = newrotation;
    }

    public void OnClickSubmit()
    {
        hasSubmit = true;
        foreach (string p in path)
        {
            if (p == "Walk")
            {
                Walk();
            }
            if (p == "Turnleft")
            {
                TurnLeft();
            }
            if (p == "TurnRight")
            {
                TurnRight();
            }
        }
        path.Clear();
        showPath();
    }

    public void Update()
    {
        if (hasSubmit)
            if (MovementBlcok.transform.position.y <= -100)
            {
                EndGame.SetActive(true);
                GUI.SetActive(false);
                hasSubmit = false;
            }
    }

    public void OnClickReplay()
    {
        Destroy(MovementBlcok);
        Initiate();
    }

    public void OnClickCancel()
    {
        path.Clear();
        showPath();
    }
    


}
