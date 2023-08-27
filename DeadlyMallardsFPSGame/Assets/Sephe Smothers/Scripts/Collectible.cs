using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour, Interactables
{

    public GameObject objectToCollect;
    public bool DestroyGameObject;
    public bool completed;

    private void Start()
    {
        completed = false;
    }

    void Destroy()
    {
        objectToCollect.SetActive(false);
    }

    public void Interact()
    {
        if (DestroyGameObject)
        {
            Destroy();
            completed = true;
        }
        else
        {
            completed = true;
        }
    }

    public string promptUi()
    {
        return "Press 'E'";
    }
}
