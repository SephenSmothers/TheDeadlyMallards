using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmoCrate : MonoBehaviour, Interactables
{
    public void Interact()
    {
        GunsManager curGun = GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun];
        if (curGun.totalAmmo != curGun.maxAmmo && GameManager.instance.cash >= 100)
        {
            GameManager.instance.playerScript.GetMaxAmmo();
            GameManager.instance.RemoveCash(100);
        }
    }
    public string promptUi()
    {
        GunsManager curGun = GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun];
        if (curGun.totalAmmo != curGun.maxAmmo)
        {
            return "Press E to get max ammo for 100 Cash";
        }
        else
        {
            return "You have max ammo already";
        }
    }
}
