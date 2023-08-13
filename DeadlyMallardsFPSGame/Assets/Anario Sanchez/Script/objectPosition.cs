using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPosition : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset;

    void Update()
    {
        transform.position = cameraTransform.position + offset;
    }
}
