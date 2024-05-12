using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Debugging : NetworkBehaviour
{
    private Text cheatText;
    [SerializeField] private GameManager gameManager;
    private FirstPersonController localFPC;
    private Inventory localPlayerInventory;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
            gameManager.OnCheatStateChange += HandleCheatStateChange;
            cheatText = gameObject.GetComponentInChildren<Text>();
            localFPC = NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<FirstPersonController>();
            localPlayerInventory = NetworkManager.LocalClient.PlayerObject.gameObject.GetComponentInChildren<Inventory>();
            gameObject.SetActive(false);
        }
    }

    private void HandleCheatStateChange()
    {
        if (IsOwner)
        {
            Debug.Log($"Fired Off Event");
            cheatText.text = "Cheats active :^)";
            gameObject.SetActive(true);
        }
    }

    [Command("qq-fly-mode")]
    public void SetFlyMode(bool flyMode)
    {
        if (!IsOwner)
        {
            Debug.Log($"IsOwner on {gameObject.GetComponent<NetworkObject>().OwnerClientId}: {IsOwner}");
            return;
        }

        if (GameManager.instance.CurrentCheatState > 0)
        {
            localFPC.IsFlying = flyMode;
            Debug.Log($"Set flying to: {localFPC.IsFlying}");
            localFPC.Gravity = flyMode ? 0 : 30.0f;
        }
    }

    // Commands for console
    [Command("qq-set-use-stamina", "Set if player should use stamina")]
    public void SetUseStamina(bool value)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localFPC.UseStamina = value;
            Debug.Log($"Set use stamina to: {localFPC.UseStamina}");
        }
    }

    [Command("qq-set-walk-speed", "Set player's walk speed")]
    public void SetWalkSpeed(float speed)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localFPC.WalkSpeed = speed;
            Debug.Log($"Set walk speed to: {localFPC.WalkSpeed}");
        }
    }

    [Command("qq-set-sprint-speed", "Set player's sprint speed")]
    public void SetSprintSpeed(float speed)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localFPC.SprintSpeed = speed;
            Debug.Log($"Set sprint speed to: {localFPC.SprintSpeed}");

        }
    }

    [Command("qq-set-crouch-speed", "Set player's crouch speed")]
    public void SetCrouchSpeed(float speed)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localFPC.CrouchSpeed = speed;
            Debug.Log($"Set crouch speed to: {localFPC.CrouchSpeed}");
        }
    }

    [Command("qq-set-water-amount", "Set player's water amount")]
    public void SetWaterAmount(int amount)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localPlayerInventory.WaterAmount = amount;
            HandleInventoryUpdate();
        }
    }

    [Command("qq-set-medicine-amount", "Set player's medicine amount")]
    public void SetMedicineAmount(int amount)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localPlayerInventory.MedicineAmount = amount;
            HandleInventoryUpdate();
        }
    }

    [Command("qq-set-food-amount", "Set player's food amount")]
    public void SetFoodAmount(int amount)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localPlayerInventory.FoodAmount = amount;
            HandleInventoryUpdate();
        }
    }

    [Command("qq-set-key-amount", "Set player's key amount")]
    public void SetKeyAmount(int amount)
    {
        if (!IsOwner) return;
        if (GameManager.instance.CurrentCheatState > 0)
        {
            localPlayerInventory.KeyAmount = amount;
            HandleInventoryUpdate();
        }
    }

    private void HandleInventoryUpdate()
    {
        InventoryActions.OnInventoryChange(new int[] { localPlayerInventory.WaterAmount,
                                                        localPlayerInventory.MedicineAmount, 
                                                        localPlayerInventory.FoodAmount, 
                                                        localPlayerInventory.KeyAmount });
    }
}
