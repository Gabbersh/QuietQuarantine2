using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashScript : InteractableObject
{
    private int waterAmount;
    private int medicineAmount;
    private int foodAmount;
    private int keyAmount;
    public int WaterAmount { get { return waterAmount; } }
    public int MedicineAmount { get { return medicineAmount; } }
    public int FoodAmount { get { return foodAmount; } }
    public int KeyAmount { get { return keyAmount; } }

    private string focusText = "Press E to Stash";

    public void Awake()
    {
        waterAmount = 0;
        medicineAmount = 0;
        foodAmount = 0;
        keyAmount = 0;  

    }

    public void Deposit(int water, int medicine, int food, int key)
    {
        waterAmount += water;
        medicineAmount += medicine;
        foodAmount += food;
        keyAmount += key;

    }

    public void Withdraw(int water, int medicine, int food, int key)
    {
        waterAmount -= water;
        medicineAmount -= medicine;
        foodAmount -= food;
        keyAmount -= key;
    }

    //void OnTriggerExit(Collider other)
    //{
    //    InventoryActions.OnStashInteraction(false);
    //}

    public override void OnFocus()
    {
        InventoryActions.OnInteractableFocus(focusText, true);
    }

    

    public override void OnInteract()
    {
        InventoryActions.OnStashInteraction();

    }

    public override void OnLoseFocus()
    {
        InventoryActions.OnInteractableLostFocus(false);
    }

   

    

    

}
