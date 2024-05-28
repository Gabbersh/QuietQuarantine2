using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        SceneManager.LoadScene("Menu");
    }
}
