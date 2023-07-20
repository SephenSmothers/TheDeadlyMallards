using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class upgradeBox : MonoBehaviour, Interactables
{
    public void Interact()
    {
        GunsManager curGun = GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun];
        if (!curGun.upgradedGun)
        {
            if(curGun.gunName == "Shotgun")
            {
                GameManager.instance.playerScript.gunList.Remove(curGun);
                curGun.damage *= 2;
                curGun.fireRate *= 0.8f;
                curGun.magSize *= 2;
                curGun.maxAmmo *= 2;
                curGun.bulletsPerShot = 7;
                curGun.totalAmmo = curGun.maxAmmo;
                curGun.upgradedGun = true;
                curGun.model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.magenta;
                GameManager.instance.playerScript.gunList.Insert(GameManager.instance.playerScript.selectedGun, curGun);
                GameManager.instance.playerScript.changeGunStats();
            }
            else
            {
                GameManager.instance.playerScript.gunList.Remove(curGun);
                curGun.damage *= 2;
                curGun.fireRate *= 0.8f;
                curGun.magSize *= 2;
                curGun.maxAmmo *= 2;
                curGun.totalAmmo = curGun.maxAmmo;
                curGun.upgradedGun = true;
                curGun.model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.magenta;
                GameManager.instance.playerScript.gunList.Insert(GameManager.instance.playerScript.selectedGun, curGun);
                GameManager.instance.playerScript.changeGunStats();
            }
        }
    }
    public string promptUi()
    {
        return null;
    }
}
