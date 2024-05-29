using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class WinnerItem : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNameText;


    
    public void SetWinnerName(string winnerName)
    {
        winnerName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "No Winner");
        winnerNameText.text = $"Winner: {winnerName}";
    }

    public string GetWinnerName()
    {
        return PlayerPrefs.GetString(NameSelector.PlayerNameKey, "No Winner");
    }
}
