using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Pause : MonoBehaviour
{
    GameObject PausePanel;
    public bool isPaused;

    private void Start()
    {
        PausePanel = gameObject;
        isPaused = false;
        PausePanel.SetActive(isPaused);

    }
    public void Resume()
    {
        isPaused = false;
        InventoryActions.TogglePause(false);
    }

    

    public void Exit()
    {
        Application.Quit();
    }

   
}