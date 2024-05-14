using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class InteractableObject : NetworkBehaviour
{
    //[SerializeField] private SpriteRenderer waterRenderer;
    //[SerializeField] private SpriteRenderer foodRenderer;
    //[SerializeField] private SpriteRenderer medicineRenderer;

    //protected bool alreadyCollected;

    public virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();

    //protected void Show(bool show)
    //{
    //    waterRenderer.enabled = show;
    //    foodRenderer.enabled = show;
    //    medicineRenderer.enabled = show;
    //}
}
