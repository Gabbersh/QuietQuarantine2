using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using System;
using TMPro;
using Unity.Netcode;

public class UIStashController : NetworkBehaviour
{
    private GameObject localPlayer;


    private Button buttonWaterSub;
    private Button buttonWaterAdd;
    private Button buttonSuppliesSub;
    private Button buttonSuppliesAdd;
    private Button buttonKeysSub;
    private Button buttonKeysAdd;
    private Button buttonFoodSub;
    private Button buttonFoodAdd;

    private Button buttonDeposit;
    private Button buttonWithdraw;

    private TMP_Text inventoryWaterText;
    private TMP_Text inventorySuppliesText;
    private TMP_Text inventoryFoodText;
    private TMP_Text inventoryKeyText;

    private TMP_Text stashedWaterText;
    private TMP_Text stashedSuppliesText;
    private TMP_Text stashedFoodText;
    private TMP_Text stashedKeyText;

    private TMP_Text transactionWaterText;
    private TMP_Text transactionSuppliesText;
    private TMP_Text transactionFoodText;
    private TMP_Text transactionKeyText;

    private int transactionWater;
    private int transactionSupplies;
    private int transactionFood;
    private int transactionKey;

    private int stashWater;
    private int stashSupplies;
    private int stashFood;
    private int stashKey;
   
    private int inventoryWater;
    private int inventorySupplies;
    private int inventoryFood;
    private int inventoryKey;

    private GameObject stashUI;
    private Inventory inventory;
    
    //void Awake()
    //{
    //    if (!IsOwner) return;

    //    stashUI = gameObject;

    //    inventory = FindInChildren(localPlayer, "Inventory").GetComponent<Inventory>();

    //    transactionWater = 0;
    //    transactionSupplies = 0;
    //    transactionFood = 0;
    //    transactionKey = 0;

    //    //inventoryWaterText = GameObject.Find("InventoryTextWater").GetComponent<TMP_Text>();
    //    //inventorySuppliesText = GameObject.Find("InventoryTextSupplies").GetComponent<TMP_Text>();
    //    //inventoryFoodText = GameObject.Find("InventoryTextFood").GetComponent<TMP_Text>();
    //    //inventoryKeyText = GameObject.Find("InventoryTextKey").GetComponent<TMP_Text>();

    //    //stashedWaterText = GameObject.Find("StashedTextWater").GetComponent<TMP_Text>();
    //    //stashedSuppliesText = GameObject.Find("StashedTextSupplies").GetComponent<TMP_Text>();
    //    //stashedFoodText = GameObject.Find("StashedTextFood").GetComponent<TMP_Text>();
    //    //stashedKeyText = GameObject.Find("StashedTextKey").GetComponent<TMP_Text>();

    //    //transactionWaterText = GameObject.Find("TransactionTextWater").GetComponent<TMP_Text>();
    //    //transactionSuppliesText = GameObject.Find("TransactionTextSupplies").GetComponent<TMP_Text>();
    //    //transactionFoodText = GameObject.Find("TransactionTextFood").GetComponent<TMP_Text>();
    //    //transactionKeyText = GameObject.Find("TransactionTextKey").GetComponent<TMP_Text>();
        
    //    //buttonWaterSub = GameObject.Find("ButtonSubWater").GetComponent<Button>();
    //    //buttonWaterAdd = GameObject.Find("ButtonAddWater").GetComponent<Button>();
    //    //buttonSuppliesSub = GameObject.Find("ButtonSubSupplies").GetComponent<Button>();
    //    //buttonSuppliesAdd = GameObject.Find("ButtonAddSupplies").GetComponent<Button>();
    //    //buttonFoodSub = GameObject.Find("ButtonSubFood").GetComponent<Button>();
    //    //buttonFoodAdd = GameObject.Find("ButtonAddFood").GetComponent<Button>();
    //    //buttonKeysSub = GameObject.Find("ButtonSubKey").GetComponent<Button>();
    //    //buttonKeysAdd = GameObject.Find("ButtonAddKey").GetComponent<Button>();

    //    //buttonDeposit = GameObject.Find("ButtonDeposit").GetComponent<Button>();
    //    //buttonWithdraw = GameObject.Find("ButtonWithdraw").GetComponent<Button>();

    //    inventoryWaterText = FindInChildren(localPlayer, "InventoryTextWater").GetComponent<TMP_Text>();
    //    inventorySuppliesText = FindInChildren(localPlayer, "InventoryTextSupplies").GetComponent<TMP_Text>();
    //    inventoryFoodText = FindInChildren(localPlayer, "InventoryTextFood").GetComponent<TMP_Text>();
    //    inventoryKeyText = FindInChildren(localPlayer, "InventoryTextKey").GetComponent<TMP_Text>();

    //    stashedWaterText = FindInChildren(localPlayer, "StashedTextWater").GetComponent<TMP_Text>();
    //    stashedSuppliesText = FindInChildren(localPlayer, "StashedTextSupplies").GetComponent<TMP_Text>();
    //    stashedFoodText = FindInChildren(localPlayer, "StashedTextFood").GetComponent<TMP_Text>();
    //    stashedKeyText = FindInChildren(localPlayer, "StashedTextKey").GetComponent<TMP_Text>();

    //    transactionWaterText = FindInChildren(localPlayer, "TransactionTextWater").GetComponent<TMP_Text>();
    //    transactionSuppliesText = FindInChildren(localPlayer, "TransactionTextSupplies").GetComponent<TMP_Text>();
    //    transactionFoodText = FindInChildren(localPlayer, "TransactionTextFood").GetComponent<TMP_Text>();
    //    transactionKeyText = FindInChildren(localPlayer, "TransactionTextKey").GetComponent<TMP_Text>();

    //    // Assign Button components
    //    buttonWaterSub = FindInChildren(localPlayer, "ButtonSubWater").GetComponent<Button>();
    //    buttonWaterAdd = FindInChildren(localPlayer, "ButtonAddWater").GetComponent<Button>();
    //    buttonSuppliesSub = FindInChildren(localPlayer, "ButtonSubSupplies").GetComponent<Button>();
    //    buttonSuppliesAdd = FindInChildren(localPlayer, "ButtonAddSupplies").GetComponent<Button>();
    //    buttonFoodSub = FindInChildren(localPlayer, "ButtonSubFood").GetComponent<Button>();
    //    buttonFoodAdd = FindInChildren(localPlayer, "ButtonAddFood").GetComponent<Button>();
    //    buttonKeysSub = FindInChildren(localPlayer, "ButtonSubKey").GetComponent<Button>();
    //    buttonKeysAdd = FindInChildren(localPlayer, "ButtonAddKey").GetComponent<Button>();

    //    buttonDeposit = FindInChildren(localPlayer, "ButtonDeposit").GetComponent<Button>();
    //    buttonWithdraw = FindInChildren(localPlayer, "ButtonWithdraw").GetComponent<Button>();

    //    buttonWaterSub.onClick.AddListener(SubWaterOnClick);
    //    buttonWaterAdd.onClick.AddListener(AddWaterOnClick);
    //    buttonSuppliesSub.onClick.AddListener(SubSuppliesOnClick);
    //    buttonSuppliesAdd.onClick.AddListener(AddSuppliesOnClick);
    //    buttonFoodSub.onClick.AddListener(SubFoodOnClick);
    //    buttonFoodAdd.onClick.AddListener(AddFoodOnClick);
    //    buttonKeysSub.onClick.AddListener(SubKeyOnClick);
    //    buttonKeysAdd.onClick.AddListener(AddKeyOnClick);


    //    buttonDeposit.onClick.AddListener(Deposit);
    //    buttonWithdraw.onClick.AddListener(Withdraw);

    //    InventoryActions.OnStashInteraction += OpenStash;
    //    InventoryActions.OnStashClose += ExitStash;
    //    stashUI.SetActive(false);


    //}

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;

        localPlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;

        stashUI = gameObject;

        inventory = FindInChildren(localPlayer, "Inventory").GetComponent<Inventory>();

        transactionWater = 0;
        transactionSupplies = 0;
        transactionFood = 0;
        transactionKey = 0;

        //inventoryWaterText = GameObject.Find("InventoryTextWater").GetComponent<TMP_Text>();
        //inventorySuppliesText = GameObject.Find("InventoryTextSupplies").GetComponent<TMP_Text>();
        //inventoryFoodText = GameObject.Find("InventoryTextFood").GetComponent<TMP_Text>();
        //inventoryKeyText = GameObject.Find("InventoryTextKey").GetComponent<TMP_Text>();

        //stashedWaterText = GameObject.Find("StashedTextWater").GetComponent<TMP_Text>();
        //stashedSuppliesText = GameObject.Find("StashedTextSupplies").GetComponent<TMP_Text>();
        //stashedFoodText = GameObject.Find("StashedTextFood").GetComponent<TMP_Text>();
        //stashedKeyText = GameObject.Find("StashedTextKey").GetComponent<TMP_Text>();

        //transactionWaterText = GameObject.Find("TransactionTextWater").GetComponent<TMP_Text>();
        //transactionSuppliesText = GameObject.Find("TransactionTextSupplies").GetComponent<TMP_Text>();
        //transactionFoodText = GameObject.Find("TransactionTextFood").GetComponent<TMP_Text>();
        //transactionKeyText = GameObject.Find("TransactionTextKey").GetComponent<TMP_Text>();

        //buttonWaterSub = GameObject.Find("ButtonSubWater").GetComponent<Button>();
        //buttonWaterAdd = GameObject.Find("ButtonAddWater").GetComponent<Button>();
        //buttonSuppliesSub = GameObject.Find("ButtonSubSupplies").GetComponent<Button>();
        //buttonSuppliesAdd = GameObject.Find("ButtonAddSupplies").GetComponent<Button>();
        //buttonFoodSub = GameObject.Find("ButtonSubFood").GetComponent<Button>();
        //buttonFoodAdd = GameObject.Find("ButtonAddFood").GetComponent<Button>();
        //buttonKeysSub = GameObject.Find("ButtonSubKey").GetComponent<Button>();
        //buttonKeysAdd = GameObject.Find("ButtonAddKey").GetComponent<Button>();

        //buttonDeposit = GameObject.Find("ButtonDeposit").GetComponent<Button>();
        //buttonWithdraw = GameObject.Find("ButtonWithdraw").GetComponent<Button>();

        inventoryWaterText = FindInChildren(localPlayer, "InventoryTextWater").GetComponent<TMP_Text>();
        inventorySuppliesText = FindInChildren(localPlayer, "InventoryTextSupplies").GetComponent<TMP_Text>();
        inventoryFoodText = FindInChildren(localPlayer, "InventoryTextFood").GetComponent<TMP_Text>();
        inventoryKeyText = FindInChildren(localPlayer, "InventoryTextKey").GetComponent<TMP_Text>();

        stashedWaterText = FindInChildren(localPlayer, "StashedTextWater").GetComponent<TMP_Text>();
        stashedSuppliesText = FindInChildren(localPlayer, "StashedTextSupplies").GetComponent<TMP_Text>();
        stashedFoodText = FindInChildren(localPlayer, "StashedTextFood").GetComponent<TMP_Text>();
        stashedKeyText = FindInChildren(localPlayer, "StashedTextKey").GetComponent<TMP_Text>();

        transactionWaterText = FindInChildren(localPlayer, "TransactionTextWater").GetComponent<TMP_Text>();
        transactionSuppliesText = FindInChildren(localPlayer, "TransactionTextSupplies").GetComponent<TMP_Text>();
        transactionFoodText = FindInChildren(localPlayer, "TransactionTextFood").GetComponent<TMP_Text>();
        transactionKeyText = FindInChildren(localPlayer, "TransactionTextKey").GetComponent<TMP_Text>();

        // Assign Button components
        buttonWaterSub = FindInChildren(localPlayer, "ButtonSubWater").GetComponent<Button>();
        buttonWaterAdd = FindInChildren(localPlayer, "ButtonAddWater").GetComponent<Button>();
        buttonSuppliesSub = FindInChildren(localPlayer, "ButtonSubSupplies").GetComponent<Button>();
        buttonSuppliesAdd = FindInChildren(localPlayer, "ButtonAddSupplies").GetComponent<Button>();
        buttonFoodSub = FindInChildren(localPlayer, "ButtonSubFood").GetComponent<Button>();
        buttonFoodAdd = FindInChildren(localPlayer, "ButtonAddFood").GetComponent<Button>();
        buttonKeysSub = FindInChildren(localPlayer, "ButtonSubKey").GetComponent<Button>();
        buttonKeysAdd = FindInChildren(localPlayer, "ButtonAddKey").GetComponent<Button>();

        buttonDeposit = FindInChildren(localPlayer, "ButtonDeposit").GetComponent<Button>();
        buttonWithdraw = FindInChildren(localPlayer, "ButtonWithdraw").GetComponent<Button>();

        buttonWaterSub.onClick.AddListener(SubWaterOnClick);
        buttonWaterAdd.onClick.AddListener(AddWaterOnClick);
        buttonSuppliesSub.onClick.AddListener(SubSuppliesOnClick);
        buttonSuppliesAdd.onClick.AddListener(AddSuppliesOnClick);
        buttonFoodSub.onClick.AddListener(SubFoodOnClick);
        buttonFoodAdd.onClick.AddListener(AddFoodOnClick);
        buttonKeysSub.onClick.AddListener(SubKeyOnClick);
        buttonKeysAdd.onClick.AddListener(AddKeyOnClick);


        buttonDeposit.onClick.AddListener(Deposit);
        buttonWithdraw.onClick.AddListener(Withdraw);

        InventoryActions.OnStashInteraction += OpenStash;
        InventoryActions.OnStashClose += ExitStash;
        stashUI.SetActive(false);
    }

    private void GetAmounts()
    {
        GetInventoryAmounts();
        GetStashAmounts();
        UpdateInventoryFields();
        UpdateStashFields();
    }

    private void GetStashAmounts()
    {
        int[] values = inventory.GetStashAmounts();

        stashWater = values[0];
        stashSupplies = values[1];
        stashFood = values[2];
        stashKey = values[3];
    }

    private void GetInventoryAmounts()
    {
        int[] values = inventory.GetResourceAmounts();

        inventoryWater = values[0];
        inventorySupplies = values[1];
        inventoryFood = values[2];
        inventoryKey = values[3];
    }

    private void AddSuppliesOnClick()
    {
        AddTransaction(ref transactionSupplies, ref transactionSuppliesText);
    }

    private void SubSuppliesOnClick()
    {
        SubTransaction(ref transactionSupplies, ref transactionSuppliesText);
    }

    private void AddWaterOnClick()
    {
        
        AddTransaction(ref transactionWater, ref transactionWaterText);
    }

    private void SubWaterOnClick()
    {
        
        SubTransaction(ref transactionWater, ref transactionWaterText);
    }

    private void AddFoodOnClick()
    {
        AddTransaction(ref transactionFood, ref transactionFoodText);
    }

    private void SubFoodOnClick()
    {
        SubTransaction(ref transactionFood, ref transactionFoodText);
    }

    private void AddKeyOnClick()
    {
        AddTransaction(ref transactionKey, ref transactionKeyText);
    }

    private void SubKeyOnClick()
    {
        SubTransaction(ref transactionKey, ref transactionKeyText);
    }

    private void SubTransaction(ref int transaction, ref TMP_Text transactionText)
    {
        transaction--;
        if(transaction < 0) { transaction = 0; }

        transactionText.text = transaction.ToString();
    }

    private void AddTransaction(ref int transaction, ref TMP_Text transactionText)
    {
        transaction++;
        if (transaction >= 99) { transaction = 99; }

        transactionText.text = transaction.ToString();
    }

    private void UpdateStashFields()
    {
        stashedWaterText.text = stashWater.ToString();
        stashedSuppliesText.text = stashSupplies.ToString();
        stashedFoodText.text = stashFood.ToString();
        stashedKeyText.text = stashKey.ToString();
    }

    private void UpdateInventoryFields()
    {
        inventoryWaterText.text = inventoryWater.ToString();
        inventorySuppliesText.text = inventorySupplies.ToString();
        inventoryFoodText.text = inventoryFood.ToString();
        inventoryKeyText.text = inventoryKey.ToString();
    }

    private void Deposit()
    {
        if (transactionWater >= inventoryWater) { transactionWater = inventoryWater; }
        else if(transactionWater <= 0) { transactionWater = 0; }

        if (transactionSupplies >= inventorySupplies) { transactionSupplies = inventorySupplies; }
        else if(transactionSupplies <= 0) { transactionSupplies = 0; }

        if (transactionFood >= inventoryFood) { transactionFood = inventoryFood; }
        else if(transactionFood <= 0) { transactionFood = 0; }

        if (transactionKey >= inventoryKey) { transactionKey = inventoryKey; }
        else if(transactionKey <= 0) { transactionKey = 0; }

        if((transactionWater + transactionSupplies + transactionFood + transactionKey) > 0) 
        {
            inventory.Deposit(new int[] { transactionWater, transactionSupplies, transactionFood, transactionKey });

            GetAmounts();
        }
    }

    private void Withdraw()
    {
        if (transactionWater >= stashWater) { transactionWater =stashWater; }
        else if (transactionWater <= 0) { transactionWater = 0; }

        if (transactionSupplies >= stashSupplies) { transactionSupplies = stashSupplies; }
        else if (transactionSupplies <= 0) { transactionSupplies = 0; }

        if (transactionFood >= stashFood) { transactionFood = stashFood; }
        else if (transactionFood <= 0) { transactionFood = 0; }

        if (transactionKey >= stashKey) { transactionKey = stashKey; }
        else if (transactionKey <= 0) { transactionKey = 0; }

        if(transactionWater + transactionSupplies + transactionFood + transactionKey > 0)
        {
            inventory.Withdraw(new int[] { transactionWater, transactionSupplies, transactionFood, transactionKey });

            GetAmounts();
        }
    }

    private void ExitStash()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        stashUI.SetActive(false);
    }

    private void OpenStash()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        stashUI.SetActive(true);
        GetAmounts();
    }

    public GameObject FindInChildren(GameObject gameObjectToCheck, string name)
    {
        foreach (var currentObject in gameObjectToCheck.GetComponentsInChildren<Transform>())
        {
            if (currentObject.name == name)
                return currentObject.gameObject;
        }

        return null;
    }
}
