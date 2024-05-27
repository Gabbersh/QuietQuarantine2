using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool paused = false;
    private bool disconnected = false;
    public GameObject PausePanel;


   

    public void Start()
    {
        PausePanel.SetActive(false);
    }
    public void TogglePause()
    {
        if(disconnected) return;

        paused = !paused;

        PausePanel.SetActive(paused);
        Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = paused;

        Debug.Log("Game Is Paused!");
    }

   

    

    public void Exit()
    {
        Application.Quit();
    }

   
}
