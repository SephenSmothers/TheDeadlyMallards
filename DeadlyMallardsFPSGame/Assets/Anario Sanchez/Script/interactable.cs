using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Interactables
{
    public void Interact();
}
public class interactable : MonoBehaviour
{
    public Transform interactableObject;
    public float interactableRange;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(interactableObject.position, interactableObject.forward);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, interactableRange)) 
            { 
                if(hitInfo.collider.gameObject.TryGetComponent(out Interactables interacted))
                {
                    interacted.Interact();
                }
            }
        }
    }
}
