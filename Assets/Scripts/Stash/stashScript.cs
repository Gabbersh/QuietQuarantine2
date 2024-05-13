using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stashScript : InteractableObject
{
   
    private string focusText = "Press E to Stash";

    public void Awake()
    {
        

    }

    //void OnTriggerExit(Collider other)
    //{
    //    InventoryActions.OnStashExit();
    //}

    public override void OnFocus()
    {
        InventoryActions.OnInteractableFocus(focusText, true);
    }

    public override void OnInteract()
    {
        InventoryActions.OnStashInteraction();
    }

    public override void OnLoseFocus()
    {
        InventoryActions.OnInteractableLostFocus(false);
    }

}
