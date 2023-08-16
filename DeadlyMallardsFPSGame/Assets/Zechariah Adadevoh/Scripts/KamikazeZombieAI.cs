using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class KamikazeZombieAI : MonoBehaviour, TakeDamage
{
    [SerializeField] Renderer modle;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int explostiondmg;
    [SerializeField] GameObject Boom;
    [SerializeField] AudioSource aud;

    [Header("---Audio---")]
    [SerializeField] AudioClip zombieCry;
    [SerializeField] float cryVolume;
    [SerializeField] AudioClip ZombieDeath;
    [SerializeField] float DeathVol;
    public int hp;
    bool playerInRange;
    float angleToPlayer;
    float stoppingDistanceOrig;
    Collider player;
  



    Vector3 playerDir;
    Vector3 enemyDir;
    Vector3 startingPos;

    bool destinationChosen;
    // Start is called before the first frame update
    void Start()
    {
      
        GameManager.instance.ReturnEnemyCount(1);
        aud.PlayOneShot(zombieCry, cryVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            ChasePlayer();
            anim.SetFloat("speedBoom", agent.velocity.normalized.magnitude);
        
        }

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
           
        }
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            anim.SetTrigger("jumpAttack");
            player = other;
            

        }
    }

    public void DestroyZombie()
    {
        Destroy(gameObject);
        Instantiate(Boom, gameObject.transform.position, Quaternion.identity);
        player.GetComponent<playerControl>().CanTakeDamage(explostiondmg);
        playerInRange = true;
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
        hp -= amount;
        StartCoroutine(flashDamage());
        GameManager.instance.AddScore(10);
        GameManager.instance.AddCash(10);
        aud.PlayOneShot(ZombieDeath, DeathVol);
        if (hp <= 0)
        {
            GameManager.instance.ReturnEnemyCount(-1);
            anim.SetBool("deadBoom", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Destroy(gameObject, 5);
            GameManager.instance.OnZombieKilled();

        }

    }
    
    IEnumerator flashDamage()
    {
        Color currColor = modle.materials[0].color;
        Color currColor2 = modle.materials[1].color;
        modle.materials[0].color = Color.red;
        modle.materials[1].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        modle.materials[0].color = currColor;
        modle.materials[1].color = currColor2;
    }
}