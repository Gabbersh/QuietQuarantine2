using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{

    private int waterAmount;
    private int medicineAmount;
    private int foodAmount;
    private int keyAmount;


    //private bool keyInInventory = false;


    public void Awake()
    {
        waterAmount = 0;
        medicineAmount = 0;
        foodAmount = 0;
        keyAmount = 0;
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
            case InventoryItem.InventoryItemType.Food:
                foodAmount++; 
                break;
            case InventoryItem.InventoryItemType.Key:
                keyAmount++; 
                break;
            default:
                break;
        }

        InventoryActions.OnInventoryChange(new int[] { waterAmount, medicineAmount, foodAmount, keyAmount });
        
        Debug.Log("Water: " + waterAmount + ", " + "Medicine: " +  medicineAmount + ", " + "Food: " + foodAmount + ", " + "Keys: " + keyAmount);
    }

    public void RemoveNeededResources(int foodRequired, int waterRequired, int medicineRequired)
    {
        //switch(Inventory.itemType)
        //{
        //    case InventoryItem.InventoryItemType.Water:
        //        if() 
        //        {
        //            waterAmount--;
        //        }
        //        break;
        //    case InventoryItem.InventoryItemType.Medicine:
        //        if(medicineAmount >= 3)
        //        {
        //            medicineAmount--;
        //        }
        //        break;
        //    case InventoryItem.InventoryItemType.Food:
        //        if(foodAmount >= 3)
        //        {
        //            foodAmount--;
        //        }
        //        break;
        //    default:
        //        break;
        //}

        foodAmount -= foodRequired;
        waterAmount -= waterRequired;
        medicineAmount -= medicineRequired;
    }

    

    public int[] GetResourceAmounts()
    {
        return new int[] { waterAmount, medicineAmount, foodAmount };
    }
    //public void keyInInv()
    //{
    //    if (keyInInventory = true)
    //    {

    //    }
    //}
    
}
