using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedDoors : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors;
    private List<bool> doorStates;


    private void Start()
    {
        //var interact = GameObject.Find("Interact").GetComponent<CanvasGroup>();
        //var unlocked = GameObject.Find("Unlocked").GetComponent<CanvasGroup>();
        //var locked = GameObject.Find("Locked").GetComponent<CanvasGroup>();

        //foreach (var door in GameObject.Find("Doors").GetComponentsInChildren())
        //{
        //    door.Interact = interact;
        //    door.Unlocked = unlocked;
        //    door.Locked = locked;
        //}

        doorStates = new List<bool>();

        for (int i = 0; i < 2; i++)
        {
            doorStates.Add(true);
        }
        for (int i = 0; i < 4; i++)
        {
            doorStates.Add(false);
        }

        ShuffleList(doorStates);

        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].gameObject.GetComponentInChildren<WinSceneStart>().DoorInteractable = doorStates[i];
        }

    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
