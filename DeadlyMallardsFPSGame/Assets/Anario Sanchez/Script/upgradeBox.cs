using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeBox : MonoBehaviour, Interactables
{
    public void Interact()
    {
        GunsManager curGun = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun];
        if (!curGun.upgradedGun1 && !curGun.upgradedGun2 && GameManager.instance.cash >= 2500)
        {
            if(curGun.gunName == "Shotgun")
            {
                GameManager.instance.shootingScript.gunList.Remove(curGun);
                curGun.damage *= 2;
                curGun.fireRate *= 0.8f;
                curGun.magSize *= 2;
                curGun.maxAmmo *= 2;
                curGun.bulletsPerShot = 7;
                curGun.totalAmmo = curGun.maxAmmo;
                curGun.upgradedGun1 = true;
                curGun.model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.magenta;
                GameManager.instance.shootingScript.gunList.Insert(GameManager.instance.shootingScript.selectedGun, curGun);
                GameManager.instance.shootingScript.changeGunStats();
                GameManager.instance.RemoveCash(2500);
            }
            else
            {
                GameManager.instance.shootingScript.gunList.Remove(curGun);
                curGun.damage *= 2;
                curGun.fireRate *= 0.8f;
                curGun.magSize *= 2;
                curGun.maxAmmo *= 2;
                curGun.totalAmmo = curGun.maxAmmo;
                curGun.upgradedGun1 = true;
                curGun.model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.magenta;
                GameManager.instance.shootingScript.gunList.Insert(GameManager.instance.shootingScript.selectedGun, curGun);
                GameManager.instance.shootingScript.changeGunStats();
                GameManager.instance.RemoveCash(2500);
            }
        }
        else if(curGun.upgradedGun1 && !curGun.upgradedGun2 && GameManager.instance.cash >= 5000)
        {
            if (curGun.gunName == "Shotgun")
            {
                GameManager.instance.shootingScript.gunList.Remove(curGun);
                curGun.damage *= 2;
                curGun.fireRate *= 0.8f;
                curGun.magSize *= 2;
                curGun.maxAmmo *= 2;
                curGun.bulletsPerShot = 10;
                curGun.totalAmmo = curGun.maxAmmo;
                curGun.upgradedGun2 = true;
                curGun.model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
                GameManager.instance.shootingScript.gunList.Insert(GameManager.instance.shootingScript.selectedGun, curGun);
                GameManager.instance.shootingScript.changeGunStats();
                GameManager.instance.RemoveCash(5000);
            }
            else
            {
                GameManager.instance.shootingScript.gunList.Remove(curGun);
                curGun.damage *= 2;
                curGun.fireRate *= 0.8f;
                curGun.magSize *= 2;
                curGun.maxAmmo *= 2;
                curGun.totalAmmo = curGun.maxAmmo;
                curGun.upgradedGun2 = true;
                curGun.model.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
                GameManager.instance.shootingScript.gunList.Insert(GameManager.instance.shootingScript.selectedGun, curGun);
                GameManager.instance.shootingScript.changeGunStats();
                GameManager.instance.RemoveCash(5000);
            }
        }
    }
    public string promptUi()
    {
        GunsManager curGun = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun];
        if (!curGun.upgradedGun1 && !curGun.upgradedGun2)
        {
            return $"Upgrade {curGun.gunName} for 2500 Cash";
        }
        else if (curGun.upgradedGun1 && !curGun.upgradedGun2)
        {
            return $"Upgrade {curGun.gunName} for 5000 Cash";
        }
        else
        {
            return "Gun is already Upgraded";
        }
    }
}
