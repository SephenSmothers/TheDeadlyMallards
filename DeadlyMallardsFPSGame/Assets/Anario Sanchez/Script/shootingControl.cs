using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingControl : MonoBehaviour
{
    [Header("----- Gun Stats -----")]
    public LayerMask enemy;
    public List<GunsManager> gunList = new List<GunsManager>();
    [SerializeField] GameObject gunModel;
    public Animator anim;
    [SerializeField] int damage;
    [SerializeField] float fireRate, range, spread, reloadTime, timeBetweenShots;
    [SerializeField] int magSize, bulletsPerShot, totalAmmo;
    [SerializeField] bool allowButtonHold;
    [SerializeField] GameObject muzzleFlash;
    public int selectedGun;
    private int bulletCounter;
    public GameObject bulletHolePrefab;

    public bool isShooting, readyToShoot, reloading;
    public List<GunsManager> usedGuns = new List<GunsManager>();
    [SerializeField] PlayerSoundsManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        bulletCounter = 0;
        anim.SetBool("Revolver", true);
        usedGuns.Add(gunList[selectedGun]);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance._activeMenu == null)
        {
            inputs();
            scrollGuns();
            changeGunStats();
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
            soundManager.PlayAllShots();
            shoot();
        }
        else if (gunList[selectedGun].bulletsLeft <= 0 && isShooting && !reloading)
        {

            reload();
        }

        if (gunList[selectedGun].bulletsLeft <= 0 && gunList[selectedGun].totalAmmo <= 0 && isShooting)
        {
            soundManager.PlayDryFireSound();
        }
    }
    public void reload()
    {
        reloading = true;
        anim.SetBool("isReloading", true);
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
        anim.SetBool("isReloading", false);
        GameManager.instance.UpdatePlayerUI();

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
                if (hit.collider.CompareTag("Body"))
                {
                    hit.collider.GetComponentInParent<TakeDamage>().CanTakeDamage(damage);
                }
                else if (hit.collider.CompareTag("Head"))
                {
                    int headShotDmg = damage * 2;
                    hit.collider.GetComponentInParent<TakeDamage>().CanTakeDamage(headShotDmg);
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<TakeDamage>().CanTakeDamage(damage);
                }

                if (gunList[selectedGun].hitEffect != null)
                {
                    ParticleSystem particleEffect = Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
                    particleEffect.transform.LookAt(hit.point + hit.normal);
                    Destroy(particleEffect.gameObject, 5f);
                }
            }

            if (Physics.Raycast(bullet.position, direction, out hit, 1000f) && !hit.collider.CompareTag("Head") && !hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Body"))
            {
                GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.Euler(0, 180, 0)) as GameObject;
                bulletHole.transform.LookAt(hit.point + hit.normal * .01f);
                bulletHole.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
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
            GameManager.instance.UpdatePlayerUI();
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
        if (!reloading)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
            {
                anim.SetBool(gunList[selectedGun].gunName, false);
                selectedGun++;
                changeGunStats();
                soundManager.PlaySwapGuns();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
            {
                anim.SetBool(gunList[selectedGun].gunName, false);
                selectedGun--;
                changeGunStats();
                soundManager.PlaySwapGuns();
            }
        }
    }

    public bool OnGunPickUp(GunsManager _gunStats)
    {
        if (checkGunSlots(_gunStats.gunName, gunList))
        {
            return false;
        }
        anim.SetBool(gunList[selectedGun].gunName, false);
        gunList.Add(_gunStats);
        if (gunList.Count > 2)
        {
            gunList.RemoveAt(selectedGun);
        }
        anim.SetBool(_gunStats.gunName, true);
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
        GameManager.instance.UpdatePlayerUI();
        return true;
    }

    public void changeGunStats()
    {
        anim.SetBool(gunList[selectedGun].gunName, true);
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

        GameManager.instance.UpdatePlayerUI();
    }

    public void GetMaxAmmo()
    {
        gunList[selectedGun].totalAmmo = gunList[selectedGun].maxAmmo;
        gunList[selectedGun].bulletsLeft = gunList[selectedGun].magSize;
        GameManager.instance.UpdatePlayerUI();
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
            if (guns[i].gunName == name)
            {
                return true;
            }
        }
        return false;
    }
}
