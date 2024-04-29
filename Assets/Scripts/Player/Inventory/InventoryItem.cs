using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class InventoryItem : InteractableObject
{
    [SerializeField] private InventoryItemType itemType;
    private string focusText = "Press 'E' to pick up "; 
    NetworkObject networkObject;

    public override void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        networkObject.Spawn();
        base.Awake();
    }

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
        NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<Inventory>().AddItem(itemType);
        InventoryActions.OnInteractableLostFocus(false);
        networkObject.Despawn();
    }

    public override void OnLoseFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(false);
        InventoryActions.OnInteractableLostFocus(false);
    }
}
