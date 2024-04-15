using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryItem : InteractableObject
{
    [SerializeField] private InventoryItemType itemType;

    public InventoryItemType ItemType { get { return itemType; } }
    public enum InventoryItemType
    {
        Water,
        Coin,
        Medicine
    }
    public override void OnFocus()
    {
       gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(true);
    }

    public override void OnInteract()
    {
        FirstPersonController.instance.GetComponentInChildren<Inventory>().AddItem(itemType);
        Destroy(this.gameObject);
    }

    public override void OnLoseFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(false);
    }
}
