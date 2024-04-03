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
       // throw new System.NotImplementedException();
    }

    public override void OnInteract()
    {
        FirstPersonController.instance.GetComponentInChildren<Inventory>().AddItem(itemType);
        Destroy(this.gameObject);
    }

    public override void OnLoseFocus()
    {
       // throw new System.NotImplementedException();
    }
}
