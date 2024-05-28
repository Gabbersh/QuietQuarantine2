using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] Text waterText;
    [SerializeField] Text suppliesText;
    [SerializeField] Text coinText;
    [SerializeField] Text keyText;

    GameObject inventoryUI;

    void Start()
    {
        InventoryActions.OnInventoryChange += UpdateUI;
        InventoryActions.OnInventoryToggle += ToggleUI;

        inventoryUI = gameObject;
        inventoryUI.SetActive(true);
    }

    // Update is called once per frame
    void UpdateUI(int[] amounts)
    {
        waterText.text = amounts[0].ToString();
        suppliesText.text = amounts[1].ToString();
        coinText.text = amounts[2].ToString();
        keyText.text = amounts[3].ToString();
    }

    void ToggleUI() 
    { 
        inventoryUI.SetActive(!inventoryUI.activeSelf); 
    }
}
