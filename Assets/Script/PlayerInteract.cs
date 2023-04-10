using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteract : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] coliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach( Collider col in coliderArray)
            {
                if( col.TryGetComponent(out ObjectInteract objectInteract))
                {
                    objectInteract.Interact();
                }
            }
        }
        
    }
}
