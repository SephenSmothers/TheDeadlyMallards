using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmoCrate : MonoBehaviour
{
    public GunsManager gunsManager;
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gunsManager.GetMaxAmmo();
            Destroy(gameObject);
        }
    }
}
