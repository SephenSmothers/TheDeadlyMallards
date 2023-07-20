using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buyingGuns : MonoBehaviour, Interactables
{
    public GunsManager gun;
    public void Interact()
    {
        GameManager.instance.playerScript.OnGunPickUp(gun);
    }
    public string promptUi()
    {
        return $"{gun.gunName}";
    }
}
