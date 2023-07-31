using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;

public class playerControl : MonoBehaviour, TakeDamage
{
    [Header("----- Compents -----")]
    [SerializeField] CharacterController controller;
    public LayerMask enemy;

    [Header("----- Player Stats -----")]
    public int hp;
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerSprintSpeed;
    [SerializeField] float jumpHeight;

    [Header("----- Gun Stats -----")]
    public List<GunsManager> gunList = new List<GunsManager>();
    [SerializeField] GameObject gunModel;
    [SerializeField] int damage;
    [SerializeField] float fireRate, range, spread, reloadTime, timeBetweenShots;
    [SerializeField] int magSize, bulletsPerShot, totalAmmo;
    [SerializeField] bool allowButtonHold;
    [SerializeField] GameObject muzzleFlash;
    //public ParticleSystem MuzzleFlash;
    public int selectedGun;
    private int bulletCounter;
    public GameObject bulletHolePrefab;




    //timing bools
    public bool isShooting, readyToShoot, reloading;

    Vector3 move;
    private float playerSpeed;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public int maxHP;
    public List<GunsManager> usedGuns = new List<GunsManager>();
    public MovementState state;
    private int bulletShot;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    private void Start()
    {
        maxHP = hp;
        readyToShoot = true;
        bulletCounter = 0;
        usedGuns.Add(gunList[selectedGun]);
        changeGunStats();
        UpdatePlayerUI();
        spawnPlayer();

    }
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (GameManager.instance._activeMenu == null)
        {
            inputs();
            stateCheck();
            movement();
            scrollGuns();
        }
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
        if (Input.GetKeyDown(KeyCode.R) && gunList[selectedGun].bulletsLeft < gunList[selectedGun].magSize && !reloading && gunList[selectedGun].totalAmmo > 0)
        {
            reload();
        }

        //shooting input
        if (readyToShoot && isShooting && !reloading && gunList[selectedGun].bulletsLeft > 0)
        {
            shoot();
        }
        else if (gunList[selectedGun].bulletsLeft <= 0 && Input.GetKey(KeyCode.Mouse0))
        {
            reload();
        }
    }

    public void reload()
    {
        reloading = true;
        Invoke(nameof(reloadEnd), gunList[selectedGun].reloadTime);
    }

    public void reloadEnd()
    {
        //lets everything know when reload ends do this
        int restore = gunList[selectedGun].magSize - gunList[selectedGun].bulletsLeft;
        gunList[selectedGun].totalAmmo -= restore;
        if (gunList[selectedGun].totalAmmo < 0)
        {
            gunList[selectedGun].bulletsLeft = gunList[selectedGun].magSize + gunList[selectedGun].totalAmmo;
            gunList[selectedGun].totalAmmo = 0;
        }
        else
        {
            gunList[selectedGun].bulletsLeft = gunList[selectedGun].magSize;
        }
        reloading = false;
        readyToShoot = true;
        UpdatePlayerUI();

        //gameManager.SetAmmoCount(totalAmmo);
    }

    public void shoot()
    {
        readyToShoot = false;

        if (bulletCounter < bulletsPerShot)
        {
            bulletCounter++;
            StartCoroutine(muzzleFlashTimer());



            //chooses a point where you're facing to shoot
            float x = Random.Range(-gunList[selectedGun].spread, spread);
            float y = Random.Range(-gunList[selectedGun].spread, spread);
            Vector3 direction = Camera.main.transform.forward + new Vector3(x, y, 0);

            RaycastHit hit;

            Transform bullet = transform.Find("Main Camera");
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit, gunList[selectedGun].range, enemy))
            {

                // wait for AI tag
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<TakeDamage>().CanTakeDamage(damage);
                    if (gunList[selectedGun].hitEffect != null)
                    {
                        ParticleSystem particleEffect = Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
                        particleEffect.transform.LookAt(hit.point + hit.normal);
                        Destroy(particleEffect.gameObject, 5f);
                    }
                }


            }

            if (Physics.Raycast(bullet.position, bullet.forward, out hit, 1000f) && !hit.collider.CompareTag("Enemy") || !hit.collider.CompareTag("Player"))
            {
                GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.Euler(0, 180, 0)) as GameObject;
                bulletHole.transform.LookAt(hit.point + hit.normal);
                Destroy(bulletHole, 5f);

                if (gunList[selectedGun].hitEffect != null)
                {
                    ParticleSystem particleEffect = Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
                    particleEffect.transform.LookAt(hit.point + hit.normal);
                    Destroy(particleEffect.gameObject, 5f);
                }
            }
            // Instantiate(gunEffect, shootPos.position, Quaternion.identity);
            Invoke(nameof(shoot), gunList[selectedGun].timeBetweenShots);
        }
        else
        {
            gunList[selectedGun].bulletsLeft--;
            Invoke(nameof(resetShot), gunList[selectedGun].fireRate);
            UpdatePlayerUI();
        }
    }

    public void resetShot()
    {
        readyToShoot = true;
        bulletCounter = 0;
    }

    IEnumerator muzzleFlashTimer()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }


    void scrollGuns()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGunStats();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGunStats();
        }
    }

    public bool OnGunPickUp(GunsManager _gunStats)
    {
        if (checkGunSlots(_gunStats.name, gunList))
        {
            return false;
        }
        gunList.Add(_gunStats);
        if (gunList.Count > 2)
        {
            gunList.RemoveAt(selectedGun);
        }
        damage = _gunStats.damage;
        fireRate = _gunStats.fireRate;
        range = _gunStats.range;
        spread = _gunStats.spread;
        reloadTime = _gunStats.reloadTime;
        magSize = _gunStats.magSize;
        bulletsPerShot = _gunStats.bulletsPerShot;
        totalAmmo = _gunStats.totalAmmo;
        allowButtonHold = _gunStats.allowButtonHold;

        gunModel.GetComponent<MeshFilter>().mesh = _gunStats.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().material = _gunStats.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
        if (checkGunSlots(_gunStats.name, gunList))
        {
            usedGuns.Add(_gunStats);
        }
        UpdatePlayerUI();
        return true;
    }

    public void changeGunStats()
    {
        damage = gunList[selectedGun].damage;
        range = gunList[selectedGun].range;
        fireRate = gunList[selectedGun].fireRate;
        spread = gunList[selectedGun].spread;
        reloadTime = gunList[selectedGun].reloadTime;
        magSize = gunList[selectedGun].magSize;
        bulletsPerShot = gunList[selectedGun].bulletsPerShot;
        totalAmmo = gunList[selectedGun].totalAmmo;
        allowButtonHold = gunList[selectedGun].allowButtonHold;


        gunModel.GetComponent<MeshFilter>().mesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().material = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        UpdatePlayerUI();
    }

    public void GetMaxAmmo()
    {
        gunList[selectedGun].totalAmmo = gunList[selectedGun].maxAmmo;
        gunList[selectedGun].bulletsLeft = gunList[selectedGun].magSize;
        UpdatePlayerUI();

        //gameManager.SetAmmoCount(totalAmmo);
    }

    void movement()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) +
                (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if (Input.GetButton("Jump") && groundedPlayer)
        {
            playerVelocity.y = jumpHeight;
        }
        playerVelocity.y -= 9.81f * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void stateCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift) && groundedPlayer)
        {
            state = MovementState.sprinting;
            playerSpeed = playerSprintSpeed;
        }
        else if (groundedPlayer)
        {
            state = MovementState.walking;
            playerSpeed = playerWalkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }
    public void CanTakeDamage(int amount)
    {
        hp -= amount;
        StartCoroutine(GameManager.instance.FlashScreen());
        UpdatePlayerUI();
        if (hp <= 0)
        {
            GameManager.instance.YoLose();
        }
    }

    public void GetMaxHealth()
    {
        hp = maxHP;
        UpdatePlayerUI();
    }
    public void UpdatePlayerUI()
    {
        GameManager.instance.playerHpBar.fillAmount = (float)hp / maxHP;
        if (gunList.Count > 0)
        {
            GameManager.instance.ammoCountRemaning.SetText($"{gunList[selectedGun].bulletsLeft} / {gunList[selectedGun].totalAmmo}");
        }
    }
    void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance._playerSpawn.transform.position;
        controller.enabled = true;
        hp = maxHP;
        UpdatePlayerUI();
    }
    public void resetGuns()
    {
        for (int i = 0; i < usedGuns.Count; i++)
        {
            usedGuns[i].resetGunStats();
        }
    }
    public bool checkGunSlots(string name, List<GunsManager> guns)
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i].name == name)
            {
                return true;
            }
        }
        return false;
    }
}
