using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] Slider staminaBar;
    [SerializeField] FirstPersonController controller;

    void Start()
    {
        staminaBar.maxValue = controller.GetmaxStamina;
        staminaBar.value = controller.GetmaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.value = controller.GetCurrentStamina;
    }
}
