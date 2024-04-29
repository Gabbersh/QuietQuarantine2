using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class InventoryItem : InteractableObject
{
    [SerializeField] private InventoryItemType itemType;
    private string focusText = "Press 'E' to pick up ";
    private NetworkVariable<bool> isObjectEnabled = new NetworkVariable<bool>(true);

    public override void Awake()
    {
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

    public void FixedUpdate()
    {
        if (!isObjectEnabled.Value)
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(true);
        InventoryActions.OnInteractableFocus(focusText + itemType.ToString(), true);
    }

    public override void OnInteract()
    {
        NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponentInChildren<Inventory>().AddItem(itemType);
        InventoryActions.OnInteractableLostFocus(false);

        if (IsServer)
        {
            isObjectEnabled.Value = false;
        }
        else
        {
            ToggleEnableServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleEnableServerRpc()
    {
        isObjectEnabled.Value = false;
    }

    public override void OnLoseFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(false);
        InventoryActions.OnInteractableLostFocus(false);
    }
}
