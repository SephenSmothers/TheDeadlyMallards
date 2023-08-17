using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class dropChance : MonoBehaviour
{
    public GameObject healthCrate;
    public GameObject ammoCrate;
    public GameObject dynamite;

    public void randomChance()
    {
        int chance = Random.Range(1, 101);
        if (chance <= 10)
        {
            Instantiate(healthCrate, transform.position + Vector3.up, Quaternion.identity);
        }
        else if (chance <= 20)
        {
            Instantiate(ammoCrate, transform.position +  Vector3.up, Quaternion.identity);
        }
        else if(chance <= 30)
        {
            Instantiate(dynamite, transform.position + Vector3.up, Quaternion.Euler(0f,0f,-90f));
        }
    }
}
