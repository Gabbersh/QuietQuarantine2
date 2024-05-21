using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : NetworkBehaviour
{
    [SerializeField] Slider staminaBar;
    [SerializeField] FirstPersonController controller;

    void Start()
    {
        if (!IsOwner) return;
        //controller = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<FirstPersonController>();
        staminaBar.maxValue = controller.GetmaxStamina;
        staminaBar.value = controller.GetmaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        staminaBar.value = controller.GetCurrentStamina;
    }
}
