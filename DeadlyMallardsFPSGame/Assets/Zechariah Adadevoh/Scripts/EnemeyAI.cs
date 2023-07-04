using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemeyAI : MonoBehaviour, TakeDamage
{
    [SerializeField] Renderer modle;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int hp;

    bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (playerInRange)
        {
            agent.SetDestination(GameManager.instance._player.transform.position);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void CanTakeDamage(int amount)
    {
        StartCoroutine(flashDamage());
        hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }


    IEnumerator flashDamage()
    {
        modle.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        modle.material.color = Color.white;
    }
}
