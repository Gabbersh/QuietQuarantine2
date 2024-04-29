using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private CanvasGroup interactableUI;
    private bool playerWithInRange;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactableUI.gameObject.SetActive(true);
            playerWithInRange = true;
        }
    }

    private void Update()
    {
        if (playerWithInRange)
        {
            Activate();
        }
    }
    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWithInRange = false;
            interactableUI.gameObject.SetActive(false);
        }
    }
}
