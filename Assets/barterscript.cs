using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barterscript : InteractableObject
{
    // Start is called before the first frame update
    private int foodRequired = 3;
    private int waterRequired = 3;
    private int medicineRequired = 3;
    private int foodAmount;
    private int waterAmount;
    private int medicineAmount;
    [SerializeField] private Inventory inventory;
    

    void AttemptBarter()
    {

        int[] resourceAmounts = inventory.GetResourceAmounts();

        waterAmount = resourceAmounts[0];
        medicineAmount = resourceAmounts[1];
        foodAmount = resourceAmounts[2];

        if(HasEnoughResources(foodRequired, waterRequired, medicineRequired))
        {
            inventory.RemoveNeededResources(foodRequired, waterRequired, medicineRequired);

            inventory.AddItem(InventoryItem.InventoryItemType.Key);

            Debug.Log("Barter successful! You recieved a key.");
        } 
        
        else
        {
            Debug.Log("Not enough resources:(");
        }
    }
    public bool HasEnoughResources(int foodRequired, int waterRequired, int medicineRequired)
    {
        return (foodAmount >= foodRequired && waterAmount >= waterRequired && medicineAmount >= medicineRequired);
    }

    public override void OnInteract()
    {
        AttemptBarter();
    }

    public override void OnFocus()
    {
        
    }

    public override void OnLoseFocus()
    {
        
    }
}
