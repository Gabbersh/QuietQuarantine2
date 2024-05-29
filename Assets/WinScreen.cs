using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text winMessage;

    private void Start()
    {
        string playerName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Player");
        winMessage.text = $"Congrats to {playerName} for escaping!";
    }
}
