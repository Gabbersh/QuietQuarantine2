using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : NetworkBehaviour
{
    [SerializeField] Slider staminaBar;
    [SerializeField] FirstPersonController controller;
    private GameObject staminaUI;
    void Start()
    {
        if (!IsOwner) return;
        staminaUI = gameObject;
        //controller = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<FirstPersonController>();
        staminaBar.maxValue = controller.GetmaxStamina;
        staminaBar.value = controller.GetmaxStamina;
        UIActions.OnStaminaOpen += OnStaminaOpen;
        UIActions.OnStaminaClose += OnStaminaClose;
        staminaUI?.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        staminaBar.value = controller.GetCurrentStamina;
    }

    private void OnStaminaOpen()
    {
        staminaUI?.SetActive(true);
        if (!IsOwner) return;
        staminaBar.value = controller.GetCurrentStamina;
    }
    private void OnStaminaClose()
    {
        staminaUI?.SetActive(false);
    }
}
