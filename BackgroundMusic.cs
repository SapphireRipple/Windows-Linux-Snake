using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{    
    public AudioClip[] backgroundMusicOptions;
    public AudioSource _AudioSource;
    private int ClipIndex = 0;
    public TextMeshProUGUI MusicText;
    [SerializeField] private Slider slider;
    // Start is called before the first frame update
    private static BackgroundMusic instance = null;
    public static BackgroundMusic Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        _AudioSource.clip = backgroundMusicOptions[ClipIndex];
        _AudioSource.Play();
        // MusicText.text = backgroundMusicOptions[ClipIndex].name;
        // slider.onValueChanged.AddListener((v)=>
        //     {
        //         _AudioSource.volume = v;
        //     }
        // );

    }
    public void RightMusicButton() {
        ClipIndex = (ClipIndex + 1) % backgroundMusicOptions.Length;
        MusicText.text = backgroundMusicOptions[ClipIndex].name;
        _AudioSource.clip = backgroundMusicOptions[ClipIndex];
        _AudioSource.Play();
    }
    public void LeftMusicButton() {
        ClipIndex = (ClipIndex - 1 + backgroundMusicOptions.Length) % backgroundMusicOptions.Length;
        MusicText.text = backgroundMusicOptions[ClipIndex].name;
        _AudioSource.clip = backgroundMusicOptions[ClipIndex];
        _AudioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
