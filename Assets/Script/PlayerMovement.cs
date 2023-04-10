using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 0.1f;
    public void Movement()
    {

        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f,zDirection);

        transform.position = moveDirection * Speed;

    }

}
