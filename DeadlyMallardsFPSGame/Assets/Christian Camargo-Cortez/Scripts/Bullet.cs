using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, TakeDamage
{
    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

    [Header("----- Stats -----")]
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] int range;
    [SerializeField] ParticleSystem onDestroy;
    public bool TakeDamage;
    int hp = 1;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, range);
        rb.velocity = (GameManager.instance._player.transform.position - transform.position).normalized * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage damageable = other.GetComponent<TakeDamage>();

        if(damageable != null)
        {
            damageable.CanTakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void CanTakeDamage(int amount)
    {
        if (TakeDamage)
        {
            hp -= amount;
            if (hp <= 0)
            {
                Instantiate(onDestroy,transform.position, transform.rotation);
                Destroy(gameObject);
                
            }
        }
    }
}
