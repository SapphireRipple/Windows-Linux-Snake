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

public class MusicUIController : MonoBehaviour
{
    public TextMeshProUGUI musicText;
    public Slider volumeSlider;

    void Start()
    {
        if (BackgroundMusic.Instance != null)
        {
            volumeSlider.value = BackgroundMusic.Instance._AudioSource.volume;
            volumeSlider.onValueChanged.AddListener((v) => BackgroundMusic.Instance._AudioSource.volume = v);
            musicText.text = BackgroundMusic.Instance._AudioSource.clip.name;
        }
    }

    void Update()
    {
        if (BackgroundMusic.Instance != null)
        {
            musicText.text = BackgroundMusic.Instance._AudioSource.clip.name;
        }
    }
}