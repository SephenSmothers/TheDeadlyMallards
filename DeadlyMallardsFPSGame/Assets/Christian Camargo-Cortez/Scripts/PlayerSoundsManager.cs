using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsManager : MonoBehaviour
{
    [Header("----- Audio Sources -----")]
    public AudioSource shootingSounds;
    public AudioSource reloadSounds;
    public AudioSource cockingSounds;
    public AudioSource footStepSounds;
    public AudioSource extraSounds;

    [Header("----- Extra Sounds -----")]
    public AudioClip[] equipGuns;

    [Header("----- Footstep Sounds -----")]

    public AudioClip[] concreteSteps, CaveSteps, woodSteps;

    [Header("----- Footstep Components -----")]
    RaycastHit hit;
    public Transform rayStart;
    public float range;
    public LayerMask ground;
    [SerializeField] float walkSpeedDelay, runSpeedDelay, tiredSpeedDelay;
    private bool isPlayingFootsteps = false;
    private bool isGrounded;

    [Header("----- Revolver Sounds -----")]
    public AudioClip revolverDryFire;
    public AudioClip revolverShoot;
    public AudioClip revolverOpenCyl;
    public AudioClip revolverCloseCyl;
    public AudioClip revolverCocking;
    public AudioClip[] revolverReload;

    [Header("----- Shotgun Sounds -----")]
    public AudioClip shotgunDryFire;
    public AudioClip[] shotgunShoot;
    public AudioClip[] shotgunReload;
    public AudioClip[] shotgunCocking;

    [Header("----- Rifle Sounds -----")]
    public AudioClip rifleDryFire;
    public AudioClip rifleShoot;
    public AudioClip[] rifleReload;
    public AudioClip rifleCocking;
    public AudioClip rifleSafetySwitch;

    [Header("----- Assault-Rifle Sounds -----")]
    public AudioClip assaultRifleDryFire;
    public AudioClip assaultRifleShoot;
    public AudioClip[] assaultRifleReload;
    public AudioClip[] assaultRifleUnload;
    public AudioClip assaultRifleSafetySwitch;

    private void Update()
    {
        Debug.DrawRay(rayStart.position, rayStart.transform.up * range * -1, Color.green);
        isGrounded = Physics.Raycast(rayStart.position, rayStart.transform.up * -1, out hit, range, ground);
        bool isMoving = FootStepChecker();

        if (isMoving && !isPlayingFootsteps)
        {
            StartCoroutine(PlayFootSteps());
            isPlayingFootsteps = true;
        }
        else if (!isMoving && isPlayingFootsteps)
        {
            StopAllCoroutines();

            isPlayingFootsteps = false;
        }

    }

    public void PlayDryFireSound()
    {
        GunsManager curGun = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun];
        if (curGun.gunName == "Revolver")
        {
            shootingSounds.PlayOneShot(revolverDryFire);
        }
        else if (curGun.gunName == "AssaultRifle")
        {
            shootingSounds.PlayOneShot(assaultRifleDryFire);
        }
        else if (curGun.gunName == "Shotgun")
        {
            shootingSounds.PlayOneShot(shotgunDryFire);
        }
        else if (curGun.gunName == "SniperRifle")
        {
            shootingSounds.PlayOneShot(rifleDryFire);
        }
    }

    public void PlayAllShots()
    {
        GunsManager curGun = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun];
        if (curGun.gunName == "Revolver")
        {
            PlayRevolverShootingSound();
        }
        else if (curGun.gunName == "AssaultRifle")
        {
            PlayAssaultRifleShoot();
        }
        else if (curGun.gunName == "Shotgun")
        {
            PlayShotgunShootingSound();
        }
        else if (curGun.gunName == "SniperRifle")
        {
            PlayRifleShootingSound();
        }
        else if (curGun.gunName == "GattlingGun")
        {
            PlayAssaultRifleShoot();
        }
    }

    //REVOLVER SOUNDS
    public void PlayRevolverShootingSound()
    {
            shootingSounds.PlayOneShot(revolverShoot);
        
    }

    public void PlayRevolverReloadSound()
    {
        shootingSounds.PlayOneShot(revolverReload[Random.Range(0, revolverReload.Length)]);
    }

    public void PlayRevolverOpenCyl()
    {
        reloadSounds.PlayOneShot(revolverOpenCyl);
    }

    public void PlayRevolverCloseCyl()
    {
        reloadSounds.PlayOneShot(revolverCloseCyl);
    }

    public void PlayRevolverCocking()
    {
        cockingSounds.PlayOneShot(revolverCocking);
    }

    //SHOTGUN SOUNDS
    public void PlayShotgunShootingSound()
    {
        shootingSounds.PlayOneShot(shotgunShoot[Random.Range(0, shotgunShoot.Length)]);
    }
    public void PlayShotgunReload()
    {
        reloadSounds.PlayOneShot(shotgunReload[Random.Range(0, shotgunReload.Length)]);
    }

    public void PlayShotGunCocking()
    {
        cockingSounds.PlayOneShot(shotgunCocking[Random.Range(0, shotgunCocking.Length)]);
    }

    //RIFLE SOUNDS
    public void PlayRifleShootingSound()
    {
        shootingSounds.PlayOneShot(rifleShoot);
    }

    public void PlayRifleReload()
    {
        reloadSounds.PlayOneShot(rifleReload[Random.Range(0, rifleReload.Length)]);
    }

    public void PlayRifleCocking()
    {
        cockingSounds.PlayOneShot(rifleCocking);
    }

    public void PlayRifleSafetySwitch()
    {
        reloadSounds.PlayOneShot(rifleSafetySwitch);
    }

    //ASSAULT-RIFLE SOUNDS
    public void PlayAssaultRifleShoot()
    {
            shootingSounds.PlayOneShot(assaultRifleShoot);
    }

    public void PlayAssaultRifleReload()
    {
        reloadSounds.PlayOneShot(assaultRifleReload[Random.Range(0, assaultRifleReload.Length)]);
    }

    public void PlayAssaultRifleUnload()
    {
        reloadSounds.PlayOneShot(assaultRifleUnload[Random.Range(0, assaultRifleUnload.Length)]);
    }

    public void PlayAssaultRifleSafetySwitch()
    {
        cockingSounds.PlayOneShot(assaultRifleSafetySwitch);
    }

    //FOOT STEPS
    public IEnumerator PlayFootSteps()
    {
        while (true)
        {
            if (FootStepChecker() && isGrounded)
            {
                PlayFootstepSound();
                yield return new WaitForSeconds(GetInterval());
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void PlayFootstepSound()
    {
        if (Physics.Raycast(rayStart.position, rayStart.transform.up * -1, out hit, range, ground))
        {
            if (hit.collider.CompareTag("Concrete"))
            {
                GetFootstepSound(concreteSteps);
            }
            else if (hit.collider.CompareTag("CaveGround"))
            {
                GetFootstepSound(CaveSteps);
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                GetFootstepSound(woodSteps);
            }
            else
            {
                GetFootstepSound(concreteSteps);
            }
        }

    }
    private void GetFootstepSound(AudioClip[] clips)
    {
        footStepSounds.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    private bool FootStepChecker()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }

    private float GetInterval()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !GameManager.instance.playerScript.isTiredChecker())
        {
            return runSpeedDelay;
        }
        else if(GameManager.instance.playerScript.isTiredChecker())
        {
            return tiredSpeedDelay;
        }
        else
        {
            return walkSpeedDelay;
        }
    }

    //EQUIPMENT 
    public void PlaySwapGuns()
    {
        extraSounds.PlayOneShot(equipGuns[Random.Range(0, equipGuns.Length)]);
    }

}
