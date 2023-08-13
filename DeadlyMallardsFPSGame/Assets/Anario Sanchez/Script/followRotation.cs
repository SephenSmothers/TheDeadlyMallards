using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followRotation : MonoBehaviour
{
    public Transform rot;
    void Update()
    {
        transform.rotation = rot.rotation;
    }
}
