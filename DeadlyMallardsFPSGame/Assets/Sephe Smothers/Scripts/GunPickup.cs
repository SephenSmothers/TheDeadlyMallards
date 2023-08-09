using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunsManager gun;

    // Start is called before the first frame update
    void Start()
    {
        gun.bulletsLeft = gun.maxAmmo;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.shootingScript.OnGunPickUp(gun);

            Destroy(gameObject);
        }
    }
}
