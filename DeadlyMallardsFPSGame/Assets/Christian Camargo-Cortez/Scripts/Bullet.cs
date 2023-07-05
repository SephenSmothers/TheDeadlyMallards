using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

    [Header("----- Stats -----")]
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] int range;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, range);
        rb.velocity = transform.forward * bulletSpeed;
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
}
