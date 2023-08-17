using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamiteThrowable : MonoBehaviour
{
    public GameObject dynamite;
    public Transform cameraPosition;
    public int dynamiteAmount;
    public int dynamiteMaxAmount;
    private void Start()
    {
        dynamiteAmount = 1;
        dynamiteMaxAmount = 3;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && dynamiteAmount > 0)
        {
            var curDynamite = Instantiate(dynamite, cameraPosition.position + cameraPosition.forward, Quaternion.identity);
            curDynamite.GetComponent<Rigidbody>().velocity = cameraPosition.forward * 10f;
            dynamiteAmount -= 1;
        }
    }
    public void moreDynamite()
    {
        if(dynamiteAmount != dynamiteMaxAmount)
        {
            dynamiteAmount += 1;
        }
    }
}
