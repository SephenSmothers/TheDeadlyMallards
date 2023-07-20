using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmoCrate : MonoBehaviour, Interactables
{
    public void Interact()
    {
        GunsManager curGun = GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun];
        if (curGun.totalAmmo != curGun.maxAmmo)
        {
            GameManager.instance.playerScript.GetMaxAmmo();
        }
        else
        {
            Debug.Log("You have max Ammo!");
        }
    }
    public string promptUi()
    {
        return null;
    }
}
