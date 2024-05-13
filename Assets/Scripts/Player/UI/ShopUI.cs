using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private Inventory inventory;
    private GameObject ui;

    private TMP_Text inventoryWaterText;
    private TMP_Text inventorySuppliesText;
    private TMP_Text inventoryFoodText;
    private TMP_Text inventoryKeyText;

    private TMP_Text waterCostText;
    private TMP_Text suppliesCostText;
    private TMP_Text foodCostText;
    private TMP_Text keyWaterCostText;
    private TMP_Text keySuppliesCostText;
    private TMP_Text keyFoodCostText;

    private Image waterTradeImage;
    private Image suppliesTradeImage;
    private Image foodTradeImage;

    private Button waterToggleButton;
    private Button suppliesToggleButton;
    private Button foodToggleButton;
    private Button waterTradeButton;
    private Button suppliesTradeButton;
    private Button foodTradeButton;
    private Button keyTradeButton;

    [SerializeField]private Sprite waterSprite;
    [SerializeField]private Sprite suppliesSprite;
    [SerializeField] private Sprite foodSprite;

    private Resource waterTrade;
    private Resource suppliesTrade;
    private Resource foodTrade;

    private int waterTradeIndex;
    private int suppliesTradeIndex;
    private int foodTradeIndex;

    private int waterAmount;
    private int suppliesAmount;
    private int foodAmount;
    private int keyAmount;

    void Awake()
    {
        ui = gameObject;

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        waterTrade = Resource.supplies;
        suppliesTrade = Resource.food;
        foodTrade = Resource.water;

        waterTradeIndex = 0;
        suppliesTradeIndex = 0;
        foodTradeIndex = 0;

        inventoryWaterText = GameObject.Find("InventoryTextWater").GetComponent<TMP_Text>();
        inventorySuppliesText = GameObject.Find("InventoryTextSupplies").GetComponent<TMP_Text>();
        inventoryFoodText = GameObject.Find("InventoryTextFood").GetComponent<TMP_Text>();
        inventoryKeyText = GameObject.Find("InventoryTextKey").GetComponent<TMP_Text>();

        waterCostText = GameObject.Find("CostTextWater").GetComponent<TMP_Text>();
        suppliesCostText = GameObject.Find("CostTextSupplies").GetComponent<TMP_Text>();
        foodCostText = GameObject.Find("CostTextFood").GetComponent<TMP_Text>();
        keyWaterCostText = GameObject.Find("CostTextKeyWater").GetComponent<TMP_Text>();
        keySuppliesCostText = GameObject.Find("CostTextKeySupplies").GetComponent<TMP_Text>();
        keyFoodCostText = GameObject.Find("CostTextKeyFood").GetComponent<TMP_Text>();

        waterTradeImage = GameObject.Find("ImageWaterTrade").GetComponent<Image>();
        suppliesTradeImage = GameObject.Find("ImageSuppliesTrade").GetComponent<Image>();
        foodTradeImage = GameObject.Find("ImageFoodTrade").GetComponent<Image>();

        waterToggleButton = GameObject.Find("ButtonToggleWater").GetComponent<Button>();
        suppliesToggleButton = GameObject.Find("ButtonToggleSupplies").GetComponent<Button>();
        foodToggleButton = GameObject.Find("ButtonToggleFood").GetComponent<Button>();
        waterTradeButton = GameObject.Find("ButtonTradeWater").GetComponent<Button>();
        suppliesTradeButton = GameObject.Find("ButtonTradeSupplies").GetComponent<Button>();
        foodTradeButton = GameObject.Find("ButtonTradeFood").GetComponent<Button>();
        keyTradeButton = GameObject.Find("ButtonTradeKey").GetComponent<Button>();

        waterTradeImage.sprite = suppliesSprite;
        suppliesTradeImage.sprite = foodSprite;
        foodTradeImage.sprite = waterSprite;

        waterCostText.text = ShopPrices.waterPrice.ToString();
        suppliesCostText.text = ShopPrices.suppliesPrice.ToString();
        foodCostText.text = ShopPrices.foodPrice.ToString();
        keyWaterCostText.text = ShopPrices.keyPriceWater.ToString();
        keySuppliesCostText.text = ShopPrices.keyPriceSupplies.ToString();
        keyFoodCostText.text = ShopPrices.keyPriceFood.ToString();

        waterToggleButton.onClick.AddListener(WaterTradeToggle);
        suppliesToggleButton.onClick.AddListener(SuppliesTradeToggle);
        foodToggleButton.onClick.AddListener(FoodTradeToggle);

        waterTradeButton.onClick.AddListener(WaterTrade);
        suppliesTradeButton.onClick.AddListener(SuppliesTrade);
        foodTradeButton.onClick.AddListener(FoodTrade);
        keyTradeButton.onClick.AddListener(KeyTrade);

        InventoryActions.OnShopInteract += OnShopOpen;
        InventoryActions.OnShopClose += OnShopClose;

        ui.SetActive(false);
    }

    private void OnShopOpen()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GetPlayerResources();
        CheckResourceRequirments();
        UpdateUI();
        ui.SetActive(true);
    }

    private void OnShopClose()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ui.SetActive(false);
    }

    private void GetPlayerResources()
    {
        int[] resources = inventory.GetResourceAmounts();

        waterAmount = resources[0];
        suppliesAmount = resources[1];
        foodAmount = resources[2];
        keyAmount = resources[3];
    }

    private void UpdateUI()
    {
        inventoryWaterText.text = waterAmount.ToString();
        inventorySuppliesText.text = suppliesAmount.ToString();
        inventoryFoodText.text = foodAmount.ToString();
        inventoryKeyText.text = keyAmount.ToString();
    }
    
    private void CheckResourceRequirments()
    {
        GetPlayerResources();
        CheckWaterTradeRequirement();
        CheckSuppliesTradeRequirement();
        CheckFoodTradeRequirement();
        CheckKeyTradeRequirment();
    }

    private void CheckWaterTradeRequirement()
    {
        switch (waterTrade)
        {
            case Resource.supplies:
                if (suppliesAmount >= ShopPrices.waterPrice)
                {
                    waterTradeButton.interactable = true;
                }
                else { waterTradeButton.interactable = false; } 
                break;

            case Resource.food:
                if(foodAmount >= ShopPrices.waterPrice)
                {
                    waterTradeButton.interactable= true;
                }
                else { waterTradeButton.interactable = false; }
                break;

            default: 
                break;
        }
    }

    private void CheckSuppliesTradeRequirement()
    {
        switch (suppliesTrade)
        {
            case Resource.water:
                if(waterAmount >= ShopPrices.suppliesPrice)
                {
                    suppliesTradeButton.interactable = true;
                }
                else { suppliesTradeButton.interactable = false; }
                break;

            case Resource.food:
                if(foodAmount >= ShopPrices.suppliesPrice)
                {
                    suppliesTradeButton.interactable = true;
                }
                else { suppliesTradeButton.interactable = false;}
                break;

            default: break;
        }
    }

    private void CheckFoodTradeRequirement()
    {
        switch (foodTrade)
        {
            case Resource.water:
                if (waterAmount >= ShopPrices.foodPrice)
                {
                    foodTradeButton.interactable = true;
                }
                else { foodTradeButton.interactable = false; }
                break;

            case Resource.supplies:
                if (suppliesAmount >= ShopPrices.foodPrice)
                {
                    foodTradeButton.interactable = true;
                }
                else { foodTradeButton.interactable = false; }
                break;

            default: break;
        }
    }

    private void CheckKeyTradeRequirment()
    {
        if ((waterAmount >= ShopPrices.keyPriceWater) && (suppliesAmount >= ShopPrices.keyPriceSupplies) && (foodAmount >= ShopPrices.keyPriceFood))
        {
            keyTradeButton.interactable = true;
        }
        else {  keyTradeButton.interactable = false;}
    }

    private void Trade(Resource Receive) 
    {
        switch(Receive)
        {
            case Resource.water:
                inventory.Trade(new[] { waterTrade }, Receive, new[] { ShopPrices.waterPrice });
                break;

            case Resource.supplies:
                inventory.Trade(new[] { suppliesTrade }, Receive, new[] { ShopPrices.suppliesPrice });
                break;

            case Resource.food:
                inventory.Trade(new[] { foodTrade }, Receive, new[] { ShopPrices.foodPrice });
                break;

            case Resource.key:
                inventory.Trade(new[] { Resource.water, Resource.supplies, Resource.food }, Receive, new[] { ShopPrices.keyPriceWater, ShopPrices.keyPriceSupplies, ShopPrices.keyPriceFood });
                break;
            default:
                break;
        }
    }

    private void WaterTrade()
    {
        Trade(Resource.water);
        CheckResourceRequirments();
    }

    private void SuppliesTrade() 
    {
        Trade(Resource.supplies);
        CheckResourceRequirments();
    }

    private void FoodTrade()
    {
        Trade(Resource.food);
        CheckResourceRequirments();
    }

    private void KeyTrade()
    {
        Trade(Resource.key);
        CheckResourceRequirments();
    }

    private void WaterTradeToggle()
    {
        waterTradeIndex = (waterTradeIndex + 1) % 2;

        switch(waterTradeIndex)
        {
            case 0:
                waterTrade = Resource.supplies;
                waterTradeImage.sprite = suppliesSprite;
                break;
            case 1:
                waterTrade = Resource.food;
                waterTradeImage.sprite = foodSprite;
                break;
            default:
                break;
        }

        CheckWaterTradeRequirement();
    }

    private void SuppliesTradeToggle()
    {
        suppliesTradeIndex = (suppliesTradeIndex + 1) % 2;

        switch(suppliesTradeIndex)
        {
            case 0:
                suppliesTrade = Resource.food;
                suppliesTradeImage.sprite = foodSprite;
                break;
            case 1:
                suppliesTrade = Resource.water;
                suppliesTradeImage.sprite= waterSprite;
                break;
            default : 
                break;
        }

        CheckSuppliesTradeRequirement();
    }

    private void FoodTradeToggle()
    {
        foodTradeIndex = (foodTradeIndex + 1) % 2;

        switch(foodTradeIndex)
        {
            case 0:
                foodTrade = Resource.water;
                foodTradeImage.sprite = waterSprite;
                break;
            case 1:
                foodTrade = Resource.supplies;
                foodTradeImage.sprite = suppliesSprite;
                break;
            default :
                break;
        }

        CheckFoodTradeRequirement();    
    }
}
