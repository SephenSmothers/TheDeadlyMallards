using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthCrate : MonoBehaviour, Interactables
{
    public void Interact()
    {
        if (GameManager.instance.playerScript.hp != GameManager.instance.playerScript.maxHP)
        {
            GameManager.instance.playerScript.GetMaxHealth();
        }
        else
        {
            Debug.Log("You have full health!");
        }
    }
    public string promptUi()
    {
        return null;
    }
}
