using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using System;
using TMPro;

public class UIStashController : MonoBehaviour
{
    private Button buttonWaterSub;
    private Button buttonWaterAdd;
    private Button buttonSuppliesSub;
    private Button buttonSuppliesAdd;

    private Button buttonDeposit;

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

    void Awake()
    {
        transactionWater = 0;
        transactionSupplies = 0;
        transactionFood = 0;
        transactionKey = 0;

        inventoryWaterText = GameObject.Find("InventoryTextWater").GetComponent<TMP_Text>();
        inventorySuppliesText = GameObject.Find("InventoryTextSupplies").GetComponent<TMP_Text>();
        inventoryFoodText = GameObject.Find("InventoryTextFood").GetComponent<TMP_Text>();
        inventoryKeyText = GameObject.Find("InventoryTextKey").GetComponent<TMP_Text>();

        stashedWaterText = GameObject.Find("StashedTextWater").GetComponent<TMP_Text>();
        stashedSuppliesText = GameObject.Find("StashedTextSupplies").GetComponent<TMP_Text>();
        stashedFoodText = GameObject.Find("StashedTextFood").GetComponent<TMP_Text>();
        stashedKeyText = GameObject.Find("StashedTextKey").GetComponent<TMP_Text>();

        transactionWaterText = GameObject.Find("TransactionTextWater").GetComponent<TMP_Text>();
        transactionSuppliesText = GameObject.Find("TransactionTextSupplies").GetComponent<TMP_Text>();
        transactionFoodText = GameObject.Find("TransactionTextFood").GetComponent<TMP_Text>();
        transactionKeyText = GameObject.Find("TransactionTextKey").GetComponent<TMP_Text>();
        
        buttonWaterSub = GameObject.Find("ButtonSubWater").GetComponent<Button>();
        buttonWaterAdd = GameObject.Find("ButtonAddWater").GetComponent<Button>();
        buttonSuppliesSub = GameObject.Find("ButtonSubSupplies").GetComponent<Button>();
        buttonSuppliesAdd = GameObject.Find("ButtonAddSupplies").GetComponent<Button>();
        buttonDeposit = GameObject.Find("ButtonDeposit").GetComponent<Button>();

        buttonWaterSub.onClick.AddListener(SubWaterOnClick);
        buttonWaterAdd.onClick.AddListener(AddWaterOnClick);
        buttonSuppliesSub.onClick.AddListener(SubSuppliesOnClick);
        buttonSuppliesAdd.onClick.AddListener(AddSuppliesOnClick);
        buttonDeposit.onClick.AddListener(Deposit);

        InventoryActions.OnInventoryChange += UpdateInventoryFields;
        InventoryActions.OnStashChange += UpdateStashFields;
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

    private void UpdateStashFields(int[] resources)
    {
        stashedWaterText.text = resources[0].ToString();
        stashedSuppliesText.text = resources[1].ToString();
        stashedFoodText.text = resources[2].ToString();
        stashedKeyText.text = resources[3].ToString();

        stashWater = resources[0];
        stashSupplies = resources[1];
        stashFood = resources[2];
        stashKey = resources[3];
    }

    private void UpdateInventoryFields(int[] resources)
    {
        inventoryWaterText.text = resources[0].ToString();
        inventorySuppliesText.text = resources[1].ToString();
        inventoryFoodText.text = resources[2].ToString();
        inventoryKeyText.text = resources[3].ToString();

        inventoryWater = resources[0];
        inventorySupplies = resources[1];
        inventoryFood = resources[2];
        inventoryKey = resources[3];
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

        InventoryActions.OnDeposit(new[] { transactionWater, transactionSupplies, transactionFood, transactionKey });
    }
}
