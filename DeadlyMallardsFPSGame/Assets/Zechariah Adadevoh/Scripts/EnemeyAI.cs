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
    [SerializeField] Animator anim;
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
    public GameObject DamagePopUp;

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
        if (agent.isActiveAndEnabled)
        {
            ChasePlayer();
            anim.SetFloat("speed", agent.velocity.normalized.magnitude);
            anim.SetFloat("speedZombie", agent.velocity.normalized.magnitude);
            anim.SetFloat("speedTank", agent.velocity.normalized.magnitude);
            anim.SetFloat("speedGun", agent.velocity.normalized.magnitude);

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
        hp -= amount;
        
        
        StartCoroutine(flashDamage());
        GameManager.instance.AddScore(50);
        GameManager.instance.AddCash(50);
        ScoreManager.instance.AddScore(50);
        ScoreManager.instance.UpdateScores();
        ScoreManager.instance.UpdateTotalDamageDealt(amount);
        if (hp <= 0)
        {
            GameManager.instance.ReturnEnemyCount(-1);
            ScoreManager.instance.UpdateZombiesKilled(1);
            ScoreManager.instance.UpdateScores();
            anim.SetBool("Dead", true);
            anim.SetBool("deadSpeed", true);
            anim.SetBool("deadTank", true);
            anim.SetBool("DeadGun", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            if(gameObject.GetComponent("splitZombie") as splitZombie)
            {
                GetComponent<splitZombie>().OnDeath();
            }
            Destroy(gameObject,5);
            GameManager.instance.OnZombieKilled();
        }
        else
        {
            GameObject DamagetextObject = Instantiate(DamagePopUp, transform.position + Vector3.up, Quaternion.identity);
            DamageText damageText = DamagetextObject.GetComponent<DamageText>();
            damageText.enabled = true;
            damageText.DisplayDamage(amount);
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
        anim.SetTrigger("Attack");
        anim.SetTrigger("speedAttack");
        anim.SetTrigger("tankAttack");
        Instantiate(hitbox, attackpos.position, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(shootspeed);
        isattacking = false;
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
