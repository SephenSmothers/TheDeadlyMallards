using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

[CreateAssetMenu]

public class GunsManager : ScriptableObject
{
    public string gunName;
    public int damage;
    public float fireRate, range, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerShot, totalAmmo;
    public bool allowButtonHold;

    //set bullets left to the magsize and the max ammo to the beginning total ammo
    public int bulletsLeft, maxAmmo;

    public GameObject model;
    [SerializeField] public ParticleSystem hitEffect;
    public bool upgradedGun1, upgradedGun2;
    public int gunPrice;

    int origDamage;
    float origFireRate;
    float origRange;
    float origReloadTime;
    int origBulletsPerShot;
    int origMagSize;
    int origMaxAmmo;
    public float origSpread;


    private void Awake()
    {
        origDamage = damage;
        origFireRate = fireRate;
        origRange = range;
        origReloadTime = reloadTime;
        origBulletsPerShot = bulletsPerShot;
        origMagSize = magSize;
        origMaxAmmo = maxAmmo;
        origSpread = spread;
    }
  
    public void resetGunStats()
    {
        damage = origDamage;
        fireRate = origFireRate;
        bulletsPerShot = origBulletsPerShot;
        range = origRange;
        reloadTime = origReloadTime;
        magSize = origMagSize;
        maxAmmo = origMaxAmmo;
        model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.white;
        bulletsLeft = magSize;
        totalAmmo = maxAmmo;
        upgradedGun1 = false;
        upgradedGun2 = false;
        spread = origSpread;
    }
}
