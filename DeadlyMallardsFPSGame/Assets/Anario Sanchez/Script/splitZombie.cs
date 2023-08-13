using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class splitZombie : EnemeyAI
{
    [SerializeField] GameObject zombie;
    public void OnDeath()
    {
        Instantiate(zombie,transform.position,Quaternion.identity);
        Instantiate(zombie, transform.position, Quaternion.identity);
    }
}

