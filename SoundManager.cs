using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip buttonClickSound;
    public AudioClip appleEat;
    public AudioClip deathSound; 
    private float lastPlayTime = 0f;
    public float soundCooldown = 0.3f;

    

    public void PlaySound(AudioClip clip)
    {
    if (Time.time - lastPlayTime >= soundCooldown)
    {
        audioSource.clip = clip; 
        audioSource.Play();
        lastPlayTime = Time.time;
    }    
    }
}
