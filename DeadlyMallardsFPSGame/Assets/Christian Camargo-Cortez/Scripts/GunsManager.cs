using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

[CreateAssetMenu]

public class GunsManager : ScriptableObject
{
    public int damage;
    public float fireRate, range, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerShot, totalAmmo;
    public bool allowButtonHold;

    //set bullets left to the magsize and the max ammo to the beginning total ammo
    public int bulletsLeft, maxAmmo;

    public GameObject model;
    [SerializeField] public ParticleSystem hitEffect;
   
}
