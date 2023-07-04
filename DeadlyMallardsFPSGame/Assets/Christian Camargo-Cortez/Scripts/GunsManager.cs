using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class GunsManager : MonoBehaviour
{
    [Header("----- Gun Stats -----")]
    [SerializeField] int damage;
    [SerializeField] float fireRate, range, spread, reloadTime;
    [SerializeField] int magSize, bulletsPerShot, totalAmmo;
    [SerializeField] bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //timing bools
    bool isShooting, readyToShoot, reloading;

    [Header("----- Player Stuff -----")]
    public Camera playerCam;
    public Transform shootPos;
    public RaycastHit hit;
    public LayerMask enemy;

    [Header("----- UI -----")]
    [SerializeField] TextMeshProUGUI ammoCount;

    void Start()
    {
        //stuff to prepare the player
        bulletsLeft = magSize;
        totalAmmo -= magSize;
        readyToShoot = true;
    }

    public void inputs()
    {
        if (allowButtonHold)
        {
            //Automatic guns
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            //semi-auto guns
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        //reload input
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading && totalAmmo > 0)
        {
            reload();
        }

        //shooting input
        if (readyToShoot && isShooting && !reloading && bulletsLeft > 0)
        {
            shoot();
        }
    }

    public void reload()
    {
        reloading = true;
        Invoke(nameof(reloadEnd), reloadTime);
    }

    public void reloadEnd()
    {
        //lets everything know when reload ends do this
        int restore = magSize - bulletsLeft;
        totalAmmo -= restore;
        bulletsLeft = magSize;
        reloading = false;
    }

    public void shoot()
    {
        readyToShoot = false;

        //chooses a point where you're facing to shoot
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = playerCam.transform.forward + new Vector3(x,y,0);

        if(Physics.Raycast(playerCam.transform.position, direction, out hit, range, enemy))
        {
            // wait for AI tag
            if(hit.collider.CompareTag(" "))
            {
                hit.collider.GetComponent<TakeDamage>().CanTakeDamage(damage);
            }
        }

        bulletsLeft--;
        Invoke(nameof(resetShot), fireRate);
    } 

    public void resetShot()
    {
        readyToShoot = true;
    }
    void Update()
    {
        inputs();

        //Displays the ammo count
        ammoCount.SetText(bulletsLeft + " / " + totalAmmo);
    }
}
