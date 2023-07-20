using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthCrate : MonoBehaviour, Interactables
{
    public void Interact()
    {
        if (GameManager.instance.playerScript.hp != GameManager.instance.playerScript.maxHP && GameManager.instance.cash >= 250)
        {
            GameManager.instance.playerScript.GetMaxHealth();
            GameManager.instance.RemoveCash(250);
        }
    }
    public string promptUi()
    {
        if (GameManager.instance.playerScript.hp != GameManager.instance.playerScript.maxHP)
        {
            return "Press E to get max health for 250 Cash";
        }
        else
        {
            return "You are already at full health";
        }
    }
}
