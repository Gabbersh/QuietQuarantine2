using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private RandomizedDoors randomizedDoors;

    [SerializeField] private CanvasGroup interactableUI;
    [SerializeField] private CanvasGroup lockedDoorUI;
    [SerializeField] private CanvasGroup unlockedDoorUI;
    private bool playerWithInRange;
    private bool doorInteractable = false;

    public bool DoorInteractable { get { return doorInteractable; } set { doorInteractable = value; } }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactableUI.gameObject.SetActive(true);
            LeanTween.cancel(interactableUI.gameObject);
            LeanTween.alphaCanvas(interactableUI, 1, 1);
            playerWithInRange = true;
        }
    }

    private void Start()
    {
        randomizedDoors = GameObject.FindObjectOfType<RandomizedDoors>();
    }

    private void Update()
    {
        if (playerWithInRange && Input.GetKeyUp(KeyCode.E))
        {
            Activate();
            Debug.Log($"Door is {DoorInteractable}");
            //Debug.Log("Is door 0 unlocked? " + isDoor0Unlocked);
            if (DoorInteractable == false)
            {
                lockedDoorUI.gameObject.SetActive(true);
                LeanTween.cancel(lockedDoorUI.gameObject);
                LeanTween.alphaCanvas(lockedDoorUI, 1, 1);
            }
            else
            {
                unlockedDoorUI.gameObject.SetActive(true);
                LeanTween.cancel(unlockedDoorUI.gameObject);
                LeanTween.alphaCanvas(unlockedDoorUI, 1, 1);
            }
        }
    }
    public virtual void Activate()
    {
        interactableUI.gameObject.SetActive(false);
    }

    public virtual void Deactivate()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWithInRange = false;
            //interactableUI.gameObject.SetActive(false);
            LeanTween.alphaCanvas(interactableUI, 0, 0.5f)
                .setOnComplete(UIHide);
            LeanTween.alphaCanvas(lockedDoorUI, 0, 0.5f)
                .setOnComplete(UIHide);
            LeanTween.alphaCanvas(unlockedDoorUI, 0, 0.5f)
                .setOnComplete(UIHide);
        }
    }

    private void UIHide()
    {

    }
}
