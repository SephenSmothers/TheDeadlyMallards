using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class KamikazeZombieAI : EnemeyAI
{
    [SerializeField] int explostiondmg;
    [SerializeField] GameObject Boom;
    Collider player; 
    EnemeyAI zombo;

    // Start is called before the first frame update
    void Start()
    {
        zombo = this;
        GameManager.instance.ReturnEnemyCount(1);
        zombo.aud.PlayOneShot(zombo.zombieCry, zombo.cryVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if (zombo.agent.isActiveAndEnabled)
        {
            zombo.ChasePlayer();
            zombo.anim.SetFloat("speedBoom", zombo.agent.velocity.normalized.magnitude);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zombo.anim.SetTrigger("jumpAttack");
            player = other;
        }
    }

    public void DestroyZombie()
    {
        Destroy(gameObject);
        Instantiate(Boom, gameObject.transform.position, Quaternion.identity);
        player.GetComponent<playerControl>().CanTakeDamage(explostiondmg);
        zombo.playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zombo.playerInRange = false;
            
        }
    }
}