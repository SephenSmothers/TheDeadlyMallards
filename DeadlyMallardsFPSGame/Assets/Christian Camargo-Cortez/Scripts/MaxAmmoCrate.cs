using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmoCrate : MonoBehaviour
{
    public playerControl playercont;
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playercont.GetMaxAmmo();
            Destroy(gameObject);
        }
    }
}
