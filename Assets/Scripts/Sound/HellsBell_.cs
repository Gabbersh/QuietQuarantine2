using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellsBell_ : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip dingDong;

    private float timer = 0f;
    private float interval;

    [SerializeField] private float incomming;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dingDong = Resources.Load<AudioClip>("Audio/hells-bell");

        interval = incomming;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            audioSource.PlayOneShot(dingDong);

            timer = 0f;
        }
    }
}
