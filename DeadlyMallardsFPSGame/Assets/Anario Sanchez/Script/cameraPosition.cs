using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPosition : MonoBehaviour
{
    public Transform camPosition;
    void Update()
    {
        transform.position = camPosition.position;
    }
}
