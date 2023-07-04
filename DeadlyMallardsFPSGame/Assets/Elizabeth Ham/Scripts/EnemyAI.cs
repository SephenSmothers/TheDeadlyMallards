using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;

public class EnemyAI : MonoBehaviour, TakeDamage
{

    [SerializeField] int hp;
    void Start()
    {
        GameManager.instance.ReturnEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CanTakeDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            GameManager.instance.ReturnEnemyCount(-1);
            Destroy(gameObject);
        }
    }
}
