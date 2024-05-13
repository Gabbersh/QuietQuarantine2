using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : InteractableObject
{
    string toolTip = "Press 'E' to open Shop";

    public override void OnFocus()
    {
        InventoryActions.OnInteractableFocus(toolTip, true);
    }

    public override void OnInteract()
    {
        InventoryActions.OnShopInteract();
    }

    public override void OnLoseFocus()
    {
        InventoryActions.OnInteractableLostFocus(false);   
    }

   /*private void OnTriggerExit(Collider other)
    {
        other.gameObject
        InventoryActions.OnShopClose();
    }*/
}
