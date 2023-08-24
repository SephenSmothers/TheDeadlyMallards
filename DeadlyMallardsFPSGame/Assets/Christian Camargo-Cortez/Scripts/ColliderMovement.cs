using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMovement : MonoBehaviour
{
    public CapsuleCollider headCap;
    public CapsuleCollider bodyCap;

    public void capsuleDisable()
    {
       headCap.enabled = false;
       bodyCap.enabled = false;
    }
}
