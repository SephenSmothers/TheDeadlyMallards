using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface Interactables
{
    public void Interact();
    public string promptUi();
}
public class interactable : MonoBehaviour
{
    public Transform interactableObject;
    public GameObject uiPanel;
    public TextMeshProUGUI promptText;
    public float interactableRange;

    private void Start()
    {
        uiPanel.SetActive(false);
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactableRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Interactables interacted))
            {
                promptText.text = interacted.promptUi();
                uiPanel.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interacted.Interact();
                }
            }
        }
        else
        {
            uiPanel.SetActive(false);
        }
    }
}
