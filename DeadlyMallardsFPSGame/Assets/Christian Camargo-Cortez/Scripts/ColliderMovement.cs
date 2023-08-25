using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMovement : MonoBehaviour
{
    public Collider headCap;
    public Collider bodyCap;

    public void capsuleDisable()
    {
       headCap.enabled = false;
       bodyCap.enabled = false;
    }
}
