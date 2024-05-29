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
    //public event Action<InventoryItem> OnCollect;
    private bool alreadyCollected;
    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private Collider collider;
    private Rigidbody rb;

    private float respawnTimer = 40;
    private float respawnTime;

    public override void Awake()
    {
        meshRenderers.Add(GetComponent<MeshRenderer>());
        meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        respawnTime = respawnTimer;
        base.Awake();
    }

    public override void OnNetworkSpawn()
    {
        isObjectEnabled.OnValueChanged += OnStateChanged;
    }

    private void OnStateChanged(bool previousValue, bool newValue)
    {
        if (!isObjectEnabled.Value)
        {
            rb.isKinematic = true;
            collider.enabled = false;
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
            Debug.Log("Object shouldnt show!");
        }
        else
        {
            rb.isKinematic = false;
            collider.enabled = true;
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = true;
            }
            alreadyCollected = false;
            Debug.Log("Object should show!");
        }
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
        //if (!IsServer) return;

        //if (!isObjectEnabled.Value)
        //{
        //    rb.isKinematic = true;
        //    collider.enabled = false;
        //    foreach(MeshRenderer renderer in meshRenderers)
        //    {
        //        renderer.enabled = false;
        //    }
        //}

        if(alreadyCollected)
        {
            respawnTime -= Time.deltaTime;
            if(respawnTime <= 0)
            {
                alreadyCollected = false;
                respawnTime = respawnTimer;
                ToggleEnableServerRpc(true);
                //rb.isKinematic = false;
                //collider.enabled = true;
                //foreach (MeshRenderer renderer in meshRenderers)
                //{
                //    renderer.enabled = true;
                //}
            }
        }
    }

    public override void OnFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(true);
        InventoryActions.OnInteractableFocus(focusText + itemType.ToString(), true);
    }

    public override void OnInteract()
    {

        //if (!IsServer) { Show(false); }
        if (alreadyCollected) { return; }
        else { alreadyCollected = true; }

        //OnCollect?.Invoke(this);

        NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponentInChildren<Inventory>().AddItem(itemType);
        InventoryActions.OnInteractableLostFocus(false);

        ToggleEnableServerRpc(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleEnableServerRpc(bool enabled)
    {
        isObjectEnabled.Value = enabled;
    }

    public override void OnLoseFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(false);
        InventoryActions.OnInteractableLostFocus(false);
    }
}
