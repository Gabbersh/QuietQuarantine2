using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableText : MonoBehaviour
{
    Text textTip;
    GameObject interactableText;

    void Start()
    {
        interactableText = gameObject;
        textTip = GetComponentInChildren<Text>();

        InventoryActions.OnInteractableFocus += OnFocus;
        InventoryActions.OnInteractableLostFocus += LostFocus;

        interactableText.SetActive(false);
    }

    void OnFocus(string text, bool active)
    {
        textTip.text = text;
        interactableText.SetActive(active);
    }

    void LostFocus(bool active)
    {
        interactableText?.SetActive(active);
    }
   
}
