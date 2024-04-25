using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryItem : InteractableObject
{
    [SerializeField] private InventoryItemType itemType;
    private string focusText = "Press 'E' to pick up "; 
    public InventoryItemType ItemType { get { return itemType; } }
    public enum InventoryItemType
    {
        Water,
        Food,
        Medicine,
        Key
    }
    public override void OnFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(true);
        InventoryActions.OnInteractableFocus(focusText + itemType.ToString(), true);
    }

    public override void OnInteract()
    {
        FirstPersonController.instance.GetComponentInChildren<Inventory>().AddItem(itemType);
        InventoryActions.OnInteractableLostFocus(false);
        Destroy(this.gameObject);
    }

    public override void OnLoseFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(false);
        InventoryActions.OnInteractableLostFocus(false);
    }
}
