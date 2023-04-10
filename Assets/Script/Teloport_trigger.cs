using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teloport_trigger : MonoBehaviour
{
    [SerializeField]
    private Vector3 telport_position;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = telport_position;
    }

    public void setTelportPoint(Vector3 position)
    {
        telport_position = position;
    }
}
