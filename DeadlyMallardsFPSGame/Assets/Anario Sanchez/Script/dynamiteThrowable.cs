using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamiteThrowable : MonoBehaviour
{
    public GameObject dynamite;
    public Transform cameraPosition;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            var curDynamite = Instantiate(dynamite,cameraPosition.position + cameraPosition.forward,Quaternion.identity);
            curDynamite.GetComponent<Rigidbody>().velocity = cameraPosition.forward * 10f;
        }
    }
}
