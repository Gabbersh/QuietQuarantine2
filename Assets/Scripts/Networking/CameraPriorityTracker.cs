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
    private GameObject uiObject;
    private GameObject flashObject;
    [SerializeField] private bool localPlayerAlive = true;
    [SerializeField] private bool cutscenePlaying = false;
    public bool LocalPlayerAlive { get { return localPlayerAlive; } set { localPlayerAlive = value; } } // ska s�ttas fr�n monster skript
    public bool CutscenePlaying { get { return cutscenePlaying; } set { cutscenePlaying = value; } } // ska s�ttas n�r cutscene visas snabbt (monster grab animation)
    private bool foundMapCamera = false;

    [SerializeField] private int currentCameraIndex;

    [Header("Local virtual camera")] 
    [SerializeField] private CinemachineVirtualCamera localVC; // lokala virtualCameran

    [Header("Other players virtual cameras")]
    [SerializeField] private List<CinemachineVirtualCamera> playersVC = new(); // andra spelares view

    [Header("Controls")]
    [SerializeField] private const KeyCode PreviousPlayerKey = KeyCode.Mouse0; // v�nster klick
    [SerializeField] private const KeyCode NextPlayerKey = KeyCode.Mouse1; // h�ger klick

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
        localVC = FindInChildren(NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject, "FpsVcam").GetComponent<CinemachineVirtualCamera>(); // h�mta local camera

        Debug.Log($"Found local camera!");
        var localPlayersUid = NetworkManager.Singleton.LocalClientId;

        playersVC.Clear();

        foreach (var specCamObject in GameObject.FindGameObjectsWithTag("SpectateCamera")) // h�mta allas kameror �ven lokala!
        {
            var cam = specCamObject.GetComponent<CinemachineVirtualCamera>();

            // exkludera lokala kameran f�r vi har redan den!
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

    // kommer skapa problem tror bara vi skiter i det h�r, man f�r gissa vem man kollar p�
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

    // g�m eller visa viewmodel och hud element (flashlight och all i canvasUI)
    private void ShowViewModel(bool showViewModel)
    {
        var localPlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;


        if(uiObject == null)
        uiObject = FindInChildren(localPlayer, "canvasUI");

        uiObject.SetActive(showViewModel);

        if(flashObject == null)
        flashObject = FindInChildren(localPlayer, "Flashlight");

        flashObject.SetActive(showViewModel);
    }

    // MONSTER kamera ska prioriteras h�r. alla andra kameror blir 0
    private void UnPrioritiseAll()
    {
        UnPrioritiseLocalCamera();
        foreach (var vc in playersVC)
        {
            vc.Priority = 0;
        }
    }

    // knapptryck f�r att v�lja spelare att visa
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

        currentCameraIndex = Mathf.Abs(currentCameraIndex %= playersVC.Count); // vid 3 spelare kan index bli 0 , 1 , 2 och den loopar automatiskt om man g�r utanf�r index

        GetFocusedPlayersName();
    }

    // s�tter prioritering baserat p� index. G�r bara igenom alla andra spelares kameror inte lokala
    private void PrioritiseByIndex()
    {
        for (int i = 0; i < playersVC.Count; i++)
        {
            playersVC[i].Priority = i != currentCameraIndex ? 0 : int.MaxValue;
        }
    }

    // visar lokala f�r alla andra
    private void PrioritiseLocalCamera()
    {
        localVC.Priority = int.MaxValue;
        foreach (var vc in playersVC)
        {
            vc.Priority = 0;
        }
    }

    // g�r s� lokala kamera vyn inte visas.
    private void UnPrioritiseLocalCamera()
    {
        localVC.Priority = 0;
    }

    // hitta f�rsta objektet som har ett visst namn. vet att det finns Find() men den utg�r fr�n hierarchin
    public GameObject FindInChildren(GameObject gameObjectToCheck, string name)
    {
        foreach (var currentObject in gameObjectToCheck.GetComponentsInChildren<Transform>())
        {
            if (currentObject.name == name)
                return currentObject.gameObject;
        }

        return null;
    }

    // g�r igenom alla parents upp till n�r den hittar ett object med en specifik tag
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
