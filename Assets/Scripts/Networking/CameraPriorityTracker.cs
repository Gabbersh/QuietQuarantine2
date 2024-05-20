using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using UnityEngine.SceneManagement;

public class CameraPriorityTracker : NetworkBehaviour
{
    [SerializeField] private bool localPlayerAlive = true;
    [SerializeField] private bool cutscenePlaying = false;
    public bool LocalPlayerAlive { get { return localPlayerAlive; } set { localPlayerAlive = value; } } // ska sättas från monster skript
    public bool CutscenePlaying { get { return cutscenePlaying; } set { cutscenePlaying = value; } } // ska sättas när cutscene visas snabbt (monster grab animation)
    private bool foundMapCamera = false;

    [SerializeField] private int currentCameraIndex;

    [Header("Local virtual camera")] 
    [SerializeField] private CinemachineVirtualCamera localVC; // lokala virtualCameran

    [Header("Other players virtual cameras")]
    [SerializeField] private List<CinemachineVirtualCamera> playersVC = new(); // andra spelares view

    [Header("Controls")]
    [SerializeField] private const KeyCode PreviousPlayerKey = KeyCode.Mouse0; // vänster klick
    [SerializeField] private const KeyCode NextPlayerKey = KeyCode.Mouse1; // höger klick

    //[SerializeField] private Text currentPlayerName;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;

        var playerTracker = gameObject.GetComponent<PlayerTracker>();
        // goofy
        playerTracker.OnPlayerJoined += GetAllCameras;
        playerTracker.OnPlayerLeft += GetAllCameras;
        GetAllCameras();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        var playerTracker = gameObject.GetComponent<PlayerTracker>();
        playerTracker.OnPlayerJoined -= GetAllCameras;
        playerTracker.OnPlayerLeft -= GetAllCameras;
    }

    private void GetAllCameras()
    {
        localVC = FindInChildren(NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject, "FpsVcam").GetComponent<CinemachineVirtualCamera>(); // hämta local camera

        Debug.Log($"Found local camera!");
        var localPlayersUid = NetworkManager.Singleton.LocalClientId;

        playersVC.Clear();

        foreach (var specCamObject in GameObject.FindGameObjectsWithTag("SpectateCamera")) // hämta allas kameror även lokala!
        {
            var cam = specCamObject.GetComponent<CinemachineVirtualCamera>();

            // exkludera lokala kameran för vi har redan den!
            if (FindInParents(specCamObject, "Player").GetComponent<NetworkObject>().OwnerClientId != localPlayersUid)
            {
                playersVC.Add(cam);
                Debug.Log($"Found a camera!");
            }
            else
            {
                Debug.Log($"Found local camera ignoring!");
            }
        }
        Debug.Log($"Found {playersVC.Count} cameras!");
    }

    // kommer skapa problem tror bara vi skiter i det här, man får gissa vem man kollar på
    private void GetFocusedPlayersName()
    {
        // TODO FIX NAMES MAYBE

        //currentPlayerName.text =
        //    FindInParents(playersVC[currentCameraIndex].gameObject, "Player").GetComponent<FirstPersonController>().playerName.Value.ToString();

        //var vcObject = playersVC[currentCameraIndex].gameObject;
        //currentPlayerName.text = FindInParents(vcObject, "Player").GetComponent<FirstPersonController>().playerName.Value.ToString();
    }

    private void Update()
    {
        if (localVC == null || !IsOwner) return;
        //Debug.Log($"Player is alive: {LocalPlayerAlive} and Cutscene is playing: {CutscenePlaying}");

        if (LocalPlayerAlive && !CutscenePlaying)
        {
            PrioritiseLocalCamera();
            ShowViewModel(true);
        }
        else if (!LocalPlayerAlive && !CutscenePlaying)
        {
            UnPrioritiseLocalCamera();
            HandleInput();
            PrioritiseByIndex();
            ShowViewModel(false);
        }
        else
        {
            UnPrioritiseAll();
            UnAlivePlayer(true);
            ShowViewModel(false);
        }

        if(SceneManager.GetActiveScene().name == "MainGame" && !foundMapCamera) 
        {
            playersVC.Add(GameObject.Find("CamOverMap").GetComponent<CinemachineVirtualCamera>());
            foundMapCamera = true;
        }
    }

    private void UnAlivePlayer(bool v)
    {
        localPlayerAlive = !v;
    }

    // göm eller visa viewmodel och hud element (flashlight och all i canvasUI)
    private void ShowViewModel(bool showViewModel)
    {
        var localPlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;
        //FindInChildren(localPlayer, "canvasUI").SetActive(showViewModel);
        //FindInChildren(localPlayer, "Flashlight").SetActive(showViewModel);
    }

    // MONSTER kamera ska prioriteras här. alla andra kameror blir 0
    private void UnPrioritiseAll()
    {
        UnPrioritiseLocalCamera();
        foreach (var vc in playersVC)
        {
            vc.Priority = 0;
        }
    }

    // knapptryck för att välja spelare att visa
    private void HandleInput()
    {
        if (Input.GetKeyDown(PreviousPlayerKey))
        {
            currentCameraIndex--;
        }
        else if (Input.GetKeyDown(NextPlayerKey))
        {
            currentCameraIndex++;
        }

        currentCameraIndex = Mathf.Abs(currentCameraIndex %= playersVC.Count); // vid 3 spelare kan index bli 0 , 1 , 2 och den loopar automatiskt om man går utanför index

        GetFocusedPlayersName();
    }

    // sätter prioritering baserat på index. Går bara igenom alla andra spelares kameror inte lokala
    private void PrioritiseByIndex()
    {
        for (int i = 0; i < playersVC.Count; i++)
        {
            playersVC[i].Priority = i != currentCameraIndex ? 0 : int.MaxValue;
        }
    }

    // visar lokala för alla andra
    private void PrioritiseLocalCamera()
    {
        localVC.Priority = int.MaxValue;
        foreach (var vc in playersVC)
        {
            vc.Priority = 0;
        }
    }

    // gör så lokala kamera vyn inte visas.
    private void UnPrioritiseLocalCamera()
    {
        localVC.Priority = 0;
    }

    // hitta första objektet som har ett visst namn. vet att det finns Find() men den utgår från hierarchin
    public GameObject FindInChildren(GameObject gameObjectToCheck, string name)
    {
        foreach (var currentObject in gameObjectToCheck.GetComponentsInChildren<Transform>())
        {
            if (currentObject.name == name)
                return currentObject.gameObject;
        }

        return null;
    }

    // går igenom alla parents upp till när den hittar ett object med en specifik tag
    public GameObject FindInParents(GameObject gameObjectToCheck, string tagName)
    {
        var currentObject = gameObjectToCheck.transform;

        while (currentObject != null && currentObject.parent != null)
        {
            currentObject = currentObject.parent;
            if (currentObject.gameObject.CompareTag(tagName))
            {
                return currentObject.gameObject;
            }
        }

        return null;
    }
}
