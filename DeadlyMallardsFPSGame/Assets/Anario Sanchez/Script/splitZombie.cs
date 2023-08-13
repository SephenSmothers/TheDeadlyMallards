using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class splitZombie : MonoBehaviour, TakeDamage
{
    [Header("---- Zombie Components---")]
    [SerializeField] Renderer modle;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject hitbox;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Transform attackpos;
    [SerializeField] GameObject zombie;
    public GameObject DamagePopUp;
    [Header("-----Zombie Stats----")]
    public int hp;
    [SerializeField] float shootspeed;
    [SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;

    bool playerInRange;
    bool isattacking;
    float angleToPlayer;
    Vector3 playerDir;

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
        }
    }
    public void CanTakeDamage(int amount)
    {
        hp -= amount;
        StartCoroutine(flashDamage());
        GameManager.instance.AddScore(10);
        GameManager.instance.AddCash(1000);
        //ScoreManager.instance.UpdateTotalDamageDealt(amount);
        if (hp <= 0)
        {
            GameManager.instance.ReturnEnemyCount(-1);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Instantiate(zombie,transform.position,Quaternion.identity);
            Instantiate(zombie,transform.position,Quaternion.identity);
            Destroy(gameObject, 5);
            GameManager.instance.OnZombieKilled();
            ScoreManager.instance.UpdateZombiesKilled(1);
        }
        else
        {
            GameObject DamagetextObject = Instantiate(DamagePopUp, transform.position + Vector3.up, Quaternion.identity);
            DamageText damageText = DamagetextObject.GetComponent<DamageText>();
            damageText.enabled = true;
            damageText.DisplayDamage(amount);
        }

    }
    void ChasePlayer()
    {
        playerDir = GameManager.instance._player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        agent.SetDestination(GameManager.instance._player.transform.position);
        facePlayer();
        if (playerInRange && viewAngle > angleToPlayer)
        {
            if (!isattacking)
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
    IEnumerator attack()
    {
        isattacking = true;
        anim.SetTrigger("Attack");
        Instantiate(hitbox, attackpos.position, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(shootspeed);
        isattacking = false;
    }
    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
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

