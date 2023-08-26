using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour, TakeDamage
{
    [SerializeField] ObjectiveManager _manager;
    [SerializeField] Renderer modle;
    [SerializeField] NavMeshAgent agent;
    public int hp;
    public int maxHp;

    [SerializeField] float shootspeed;
    [SerializeField] Transform shootingpos;
    [SerializeField] Transform attackpos;

    [SerializeField] GameObject vomit;

    [SerializeField] GameObject hitbox;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;
    public Image bossHealthBar;

    bool playerInRange;
    float angleToPlayer;
    bool isattacking;
    Vector3 playerDir;
    public GameObject DamagePopUp;
    public bool shooter;
    bool isshooting;


    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            ChasePlayer();
            anim.SetFloat("BossZombie", agent.velocity.normalized.magnitude);
        }

        if(GameManager.instance.playerScript.isBossPlayer == true)
        {
            GameManager.instance.playerScript.isInvulnerable = false;
        }

        if (hp <= 0)
        {

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

        agent.SetDestination(GameManager.instance._player.transform.position);
        facePlayer();
        if (playerInRange && viewAngle > angleToPlayer)
        {
            if (!isshooting && shooter)
            {
                StartCoroutine(shoot());
            }
            //if (!isattacking && !shooter)
            //{
            //    StartCoroutine(attack());
            //}
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
        GameManager.instance.AddScore(10);
        GameManager.instance.AddCash(1000);
        bossHealthBar.fillAmount = (float)hp / (float)maxHp;
        if (hp <= 0)
        {
           // GameManager.instance.ReturnEnemyCount(-1);
            anim.SetBool("deadBoss", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GameManager.instance.FinalWin();
            Destroy(gameObject, 5);
            GameManager.instance.OnZombieKilled();
            _manager.LastObjective = true;
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
    //IEnumerator attack()
    //{
    //    isattacking = true;
    //    anim.SetTrigger("Attack");
    //    Instantiate(hitbox, attackpos.position, Quaternion.Euler(90, 0, 0));
    //    yield return new WaitForSeconds(shootspeed);
    //    isattacking = false;
    //}

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
