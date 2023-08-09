using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsManager : MonoBehaviour
{
    [Header("----- Audio Source -----")] 
    public AudioSource audioSource;

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

    public void PlayDryFireSound()
    {
        GunsManager curGun = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun];
        if (curGun.gunName == "Revolver")
        {
            audioSource.PlayOneShot(revolverDryFire);
        }
        else if(curGun.gunName == "AssaultRifle")
        {
            audioSource.PlayOneShot(assaultRifleDryFire);
        }
        else if(curGun.gunName == "Shotgun")
        {
            audioSource.PlayOneShot(shotgunDryFire);
        }
        else if(curGun.gunName == "SniperRifle")
        {
            audioSource.PlayOneShot(rifleDryFire);
        }
    }

    public void PlayShootingSound()
    {
        GunsManager curGun = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun];
        if (curGun.gunName == "Revolver")
        {
            audioSource.PlayOneShot(revolverShoot);
        }
        else if (curGun.gunName == "AssaultRifle")
        {
            audioSource.PlayOneShot(assaultRifleShoot);
        }
        else if (curGun.gunName == "Shotgun")
        {
            int randClipNum = Random.Range(0, shotgunShoot.Length);
            audioSource.PlayOneShot(shotgunShoot[randClipNum]);
        }
        else if (curGun.gunName == "SniperRifle")
        {
            audioSource.PlayOneShot(rifleShoot);
        }
    }

    //REVOLVER SOUNDS
    public void PlayRevolverReloadSound()
    {
        int randClipNum = Random.Range(0, revolverReload.Length);
        audioSource.PlayOneShot(revolverReload[randClipNum]);
    }

    public void PlayRevolverOpenCyl()
    {
        audioSource.PlayOneShot(revolverOpenCyl);
    }

    public void PlayRevolverCloseCyl()
    {
        audioSource.PlayOneShot(revolverCloseCyl);
    }

    public void PlayRevolverCocking()
    {
        audioSource.PlayOneShot(revolverCocking);
    }

    //SHOTGUN SOUNDS
    public void PlayShotgunReload()
    {
        int randClipNum = Random.Range(0, shotgunReload.Length);
        audioSource.PlayOneShot(shotgunReload[randClipNum]);
    }

    public void PlayShotGunCocking()
    {
        int randClipNum = Random.Range(0, shotgunCocking.Length);
        audioSource.PlayOneShot(shotgunCocking[randClipNum]);
    }

    //RIFLE SOUNDS
    public void PlayRifleReload()
    {
        int randClipNum = Random.Range(0, rifleReload.Length);
        audioSource.PlayOneShot(rifleReload[randClipNum]);
    }

    public void PlayRifleCocking()
    {
        audioSource.PlayOneShot(rifleCocking);
    }

    public void PlayRifleSafetySwitch()
    {
        audioSource.PlayOneShot(rifleSafetySwitch);
    }

    //ASSAULT-RIFLE SOUNDS
    public void PlayAssaultRifleReload()
    {
        int randClipNum = Random.Range(0, assaultRifleReload.Length);
        audioSource.PlayOneShot(assaultRifleReload[randClipNum]);
    }

    public void PlayAssaultRifleUnload()
    {
        int randClipNum = Random.Range(0, assaultRifleUnload.Length);
        audioSource.PlayOneShot(assaultRifleUnload[randClipNum]);
    }

    public void PlayAssaultRifleSafetySwitch()
    {
        audioSource.PlayOneShot(assaultRifleSafetySwitch);
    }
}
