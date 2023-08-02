using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface Collect
{
    public void Collect();
    public string PromptPlayer();
}
public class Collectible : MonoBehaviour
{
    public GameObject objectToCollect;
    public bool DestroyGameObject;
    public bool completed; 
    //public Transform interactableObject;
    //public GameObject uiPromt;
    //public TextMeshPro onCompleted;
    //float interactableRange = 3.0f;
    //bool ObjectiveCompleted;

    private void Start()
    {
        completed = false;
        //uiPromt.SetActive(false);
    }
    void Update()
    {
        //Ray ray = new Ray(interactableObject.position, interactableObject.forward);
        //if (Physics.Raycast(ray, out RaycastHit hitInfo, interactableRange))
        //{
        //    if (hitInfo.collider.gameObject.TryGetComponent(out Interactables interacted))
        //    {
        //        promptText.text = interacted.promptUi();
        //        uiPanel.SetActive(true);
        //        if (Input.GetKeyDown(KeyCode.E))
        //        {
        //            interacted.Interact();
        //        }
        //    }
        //}
        //else
        //{
        //    uiPanel.SetActive(false);
        //}
    }

    void Destroy()
    {
        objectToCollect.SetActive(false);
    }
}
