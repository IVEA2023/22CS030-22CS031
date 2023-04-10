using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    public ForumControl forum;
    public void Interact()
    {
        forum.forumActivate();
    }
}
