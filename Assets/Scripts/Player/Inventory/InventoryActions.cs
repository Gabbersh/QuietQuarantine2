using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryActions 
{
    public static Action<int[]> OnInventoryChange;
    public static Action OnInventoryToggle;
    public static Action<string, bool> OnInteractableFocus;
    public static Action<bool> OnInteractableLostFocus;

    public static Action<int[]> OnStashChange;
    public static Action<int[]> OnDeposit;
    public static Action<int[]> OnWithdraw;
    public static Action OnStashInteraction;
    public static Action OnStashClose;

    public static Action OnShopInteract;
    public static Action OnShopClose;
    public static Action OnDeath;
    public static Action<bool> TogglePause;  
}
