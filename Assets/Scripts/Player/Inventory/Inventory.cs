using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{

    private int waterAmount;
    private int medicineAmount;
    private int coinAmount;

    public void Awake()
    {
        waterAmount = 0;
        medicineAmount = 0;
        coinAmount = 0;
    }

    public void AddItem(InventoryItem.InventoryItemType item)
    {
        switch (item)
        {
            case InventoryItem.InventoryItemType.Water:
                waterAmount++;
                break;
            case InventoryItem.InventoryItemType.Medicine:
                medicineAmount++;
                break;
            case InventoryItem.InventoryItemType.Coin:
                coinAmount++; 
                break;
            default:
                break;
        }

        InventoryActions.OnInventoryChange(new int[] { waterAmount, medicineAmount, coinAmount });
        Debug.Log("Water: " + waterAmount + ", " + "Medicine: " +  medicineAmount + ", " + "Coins: " + coinAmount);
    }

    public void RemoveItem(InventoryItem item)
    {
        switch(item.ItemType)
        {
            case InventoryItem.InventoryItemType.Water:
                if(waterAmount >= 1) 
                {
                    waterAmount--;
                }
                break;
            case InventoryItem.InventoryItemType.Medicine:
                if(medicineAmount >= 1)
                {
                    medicineAmount--;
                }
                break;
            case InventoryItem.InventoryItemType.Coin:
                if(coinAmount >= 1)
                {
                    coinAmount--;
                }
                break;
            default:
                break;
        }
    }
}
