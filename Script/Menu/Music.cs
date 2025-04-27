using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance { get; set; }
    private AudioSource infantryMenuChannel;
    public AudioClip buttonClick;
    public AudioClip updateClick;
    public AudioClip buildClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        infantryMenuChannel = gameObject.GetComponent<AudioSource>();
        infantryMenuChannel.volume = 1f;
        infantryMenuChannel.playOnAwake = false;
    }

    public void PlayButtonClickSound()
    {
        if (!infantryMenuChannel.isPlaying)
        {
            infantryMenuChannel.PlayOneShot(buttonClick);
        }
    }
    public void PlayButtonUpdateClickSound()
    {
        if (!infantryMenuChannel.isPlaying)
        {
            infantryMenuChannel.PlayOneShot(updateClick);
        }
    }
    public void PlayButtonBuildingClickSound()
    {
        if (!infantryMenuChannel.isPlaying)
        {
            infantryMenuChannel.PlayOneShot(buildClick);
        }
    }
}
