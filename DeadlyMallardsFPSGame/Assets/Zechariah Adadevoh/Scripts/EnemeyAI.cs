using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemeyAI : MonoBehaviour, TakeDamage
{
    [SerializeField] Renderer modle;
    [SerializeField] NavMeshAgent agent;
    public int hp;

    [SerializeField] float shootspeed;
    [SerializeField] Transform shootingpos;
    [SerializeField] Transform attackpos;

    [SerializeField] GameObject vomit;

    [SerializeField] GameObject hitbox;
    [SerializeField] Transform headPos;
    [SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;

    bool playerInRange;
    float angleToPlayer;
    bool isShooting;
    float stoppingDistanceOrig;
    bool isattacking;

    Vector3 playerDir;
    Vector3 startingPos;
    bool destinationChosen;

    public bool shooter;
    bool isshooting;

   
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.ReturnEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
      

    }


    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);

    }


    void ChasePlayer()
    {

        playerDir = GameManager.instance._player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        //Debug.DrawRay(headPos.position, playerDir);
        //Debug.Log(angleToPlayer);

        agent.SetDestination(GameManager.instance._player.transform.position);
        facePlayer();
        if (playerInRange && viewAngle > angleToPlayer)
        {
            if (!isshooting && shooter)
            {
                StartCoroutine(shoot());
            }
            if (!isattacking && !shooter)
            {
                StartCoroutine(attack());
            }
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
        GameManager.instance.AddScore(10);
        GameManager.instance.AddCash(10);
        if (hp <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.ReturnEnemyCount(-1);
        }
    }
    IEnumerator shoot()
    {
        isshooting = true;
        Instantiate(vomit, shootingpos.position, transform.rotation);
        yield return new WaitForSeconds(shootspeed);
        isshooting = false;
    }
    IEnumerator attack()
    {
        isattacking = true;
        Instantiate(hitbox, attackpos.position, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(shootspeed);
        isattacking = false;
    }

    IEnumerator flashDamage()
    {
        Color currColor = modle.materials[0].color;
        modle.materials[0].color = Color.red;
        Color currColor2 = modle.materials[1].color;
        modle.materials[1].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        modle.materials[0].color = currColor;
        modle.materials[1].color = currColor2;
    }
}
