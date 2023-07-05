using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour
{
    [SerializeField] int damage;
    void Update()
    {
        Destroy(gameObject,0.25f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<playerControl>().CanTakeDamage(damage);
        }
    }
}
