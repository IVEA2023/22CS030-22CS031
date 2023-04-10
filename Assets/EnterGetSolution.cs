using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGetSolution : MonoBehaviour
{
    public GameObject solution;
    private void OnTriggerEnter(Collider other)
    {
        solution.SetActive(true);
    }
}
