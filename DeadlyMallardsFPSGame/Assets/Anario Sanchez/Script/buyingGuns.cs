using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buyingGuns : MonoBehaviour, Interactables
{
    public GunsManager gun;
    public void Interact()
    {
        if (GameManager.instance.cash >= gun.gunPrice && GameManager.instance.shootingScript.OnGunPickUp(gun))
        {
            GameManager.instance.RemoveCash(gun.gunPrice);
        }
    }
    public string promptUi()
    {
        if (!GameManager.instance.shootingScript.checkGunSlots(gun.gunName, GameManager.instance.shootingScript.gunList))
        {
            return $"{gun.gunName} for {gun.gunPrice} cash";
        }
        else
        {
            return "You already have this gun";
        }
    }
}
