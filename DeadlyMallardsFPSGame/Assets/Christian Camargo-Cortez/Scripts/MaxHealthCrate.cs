using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthCrate : MonoBehaviour
{
   public playerControl playerControl;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerControl.GetMaxHealth();
            Destroy(gameObject);
        }
    }
}
