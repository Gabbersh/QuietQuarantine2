using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Interactable : MonoBehaviour
{
    private RandomizedDoors randomizedDoors;

    [SerializeField] private CanvasGroup interactableUI;
    [SerializeField] private CanvasGroup lockedDoorUI;
    [SerializeField] private CanvasGroup unlockedDoorUI;
    [SerializeField] private CanvasGroup noKeyDoorUI;
    private bool playerWithInRange;
    private bool doorInteractable = false;

    public bool DoorInteractable { get { return doorInteractable; } set { doorInteractable = value; } }

    public CanvasGroup InteractableUI { get { return interactableUI; } set { interactableUI = value; } }
    public CanvasGroup LockedDoorUI { get { return lockedDoorUI; } set { lockedDoorUI = value; } }
    public CanvasGroup UnlockedDoorUI { get { return unlockedDoorUI; } set { unlockedDoorUI = value; } }

    public CanvasGroup NoKeyDoorUI { get { return noKeyDoorUI; } set { noKeyDoorUI = value; } }


    private List<GameObject> playersInteracted = new();

    private void OnTriggerEnter(Collider other)
    {
        if (playersInteracted.Contains(other.gameObject)) return;

        if (other.CompareTag("Player"))
        {
            var inventory = FindInChildren(other.gameObject, "Inventory").GetComponent<Inventory>();
            if (inventory.KeyAmount > 0)
            {
                interactableUI.gameObject.SetActive(true);
                LeanTween.cancel(interactableUI.gameObject);
                LeanTween.alphaCanvas(interactableUI, 1, 1);
                playerWithInRange = true;
            }
            else 
            {
                noKeyDoorUI.gameObject.SetActive(true);
                LeanTween.cancel(noKeyDoorUI.gameObject);
                LeanTween.alphaCanvas(noKeyDoorUI, 1, 1);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playersInteracted.Contains(other.gameObject)) return;

        if (playerWithInRange && Input.GetKeyUp(KeyCode.E))
        {
            Activate();
            Debug.Log($"Door is {DoorInteractable}");
            playersInteracted.Add(other.gameObject);
            FindInChildren(other.gameObject, "Inventory").GetComponent<Inventory>().KeyAmount--;

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

    private void Start()
    {
        //randomizedDoors = GameObject.FindObjectOfType<RandomizedDoors>();
        //interactableUI = GameObject.FindGameObjectWithTag("Interact").GetComponent<CanvasGroup>();
        //lockedDoorUI = GameObject.FindGameObjectWithTag("Locked").GetComponent<CanvasGroup>();
        //unlockedDoorUI = GameObject.FindGameObjectWithTag("Unlocked").GetComponent<CanvasGroup>();

        //interactableUI.gameObject.SetActive(false);
        //lockedDoorUI.gameObject.SetActive(false);
        //unlockedDoorUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        
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
            LeanTween.alphaCanvas(noKeyDoorUI, 0, 0.5f)
                .setOnComplete(UIHide);
        }
    }

    private void UIHide()
    {

    }

    public GameObject FindInChildren(GameObject gameObjectToCheck, string name)
    {
        foreach (var currentObject in gameObjectToCheck.GetComponentsInChildren<Transform>())
        {
            if (currentObject.name == name)
                return currentObject.gameObject;
        }

        return null;
    }
}
