using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using Unity.Netcode;

public class Inventory : NetworkBehaviour 
{

    private int waterAmount;
    private int medicineAmount;
    private int foodAmount;
    private int keyAmount;
    public int WaterAmount {  get { return waterAmount; } set {  waterAmount = value; } }
    public int MedicineAmount { get { return medicineAmount; } set {  medicineAmount = value; } }
    public int FoodAmount { get { return foodAmount; } set {  foodAmount = value; } }
    public int KeyAmount { get { return keyAmount; } set {  keyAmount = value; } }

    //private bool keyInInventory = false;


    public void Awake()
    {
        waterAmount = 0;
        medicineAmount = 0;
        foodAmount = 0;
        keyAmount = 0;

        InventoryActions.OnDeposit += DepositTransaction;
        InventoryActions.OnWithdraw += WithdrawTransaction;
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

        InventoryChanged();

        Debug.Log("Water: " + waterAmount + ", " + "Medicine: " +  medicineAmount + ", " + "Food: " + foodAmount + ", " + "Keys: " + keyAmount);
    }

    private void DepositTransaction(int[] resources)
    {
        waterAmount -= resources[0];
        medicineAmount -= resources[1];
        foodAmount -= resources[2];
        keyAmount -= resources[3];

        InventoryChanged();
    }

    private void WithdrawTransaction(int[] resources)
    {
        waterAmount += resources[0];
        medicineAmount += resources[1];
        foodAmount += resources[2];
        keyAmount += resources[3];

        InventoryChanged();
    }

    public void RemoveNeededResources(int foodRequired, int waterRequired, int medicineRequired)
    {
        foodAmount -= foodRequired;
        waterAmount -= waterRequired;
        medicineAmount -= medicineRequired;
    }

    

    public int[] GetResourceAmounts()
    {
        return new int[] { waterAmount, medicineAmount, foodAmount, keyAmount };
    }
    //public void keyInInv()
    //{
    //    if (keyInInventory = true)
    //    {

    //    }
    //}

    public void Trade(Resource[] trade, Resource receieve, int[] prices)
    {
        for(int i = 0; i < trade.Length; i++)
        {
            switch(trade[i])
            {
                case Resource.water:
                    waterAmount -= prices[i];
                    break;

                case Resource.supplies:
                    medicineAmount -= prices[i];
                    break;

                case Resource.food: 
                    foodAmount -= prices[i];
                    break;

                default: 
                    break;
            }
        }

        switch (receieve)
        {
            case Resource.water:
                waterAmount++;
                break;

            case Resource.supplies:
                medicineAmount++;
                break;

            case Resource.food:
                foodAmount++;
                break;

            case Resource.key:
                keyAmount++;
                break;

            default : 
                break;
        }
    }

    private void InventoryChanged()
    {
        InventoryActions.OnInventoryChange(new int[] { waterAmount, medicineAmount, foodAmount, KeyAmount });
    }
    
}
