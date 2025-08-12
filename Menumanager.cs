using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Menumanager : MonoBehaviour
{
     
    public float Speed = 0.6f;
    
    private bool displayStartMenu = false;
    public GameObject StartMenu;

    public TextMeshProUGUI DifficultyText;
    
    private string[] options = { "Easy", "Medium", "Hard" };
    private string[] sizes = { "Small", "Medium", "Large" };
    private string[] skinOptions = { "1","2","3","4","5","6"};
    private string[] obstacleOptions = { "Obstacles", "No Obstacles" };
    public TextMeshProUGUI obstacleText;
    private int obstacleIndex;
    private int DifficultyIndex = 0;
    private int BoardIndex = 0;
    public TextMeshProUGUI BoardText;
    public GameObject credits;
    public GameObject settings;
    
    private int SkinIndex = 0;
    public GameObject[] skins;
    string username;
    string userPath;
    public GameObject startButton;
    public GameObject LeftArrow;
    private bool settingsGoBack = false;
    private bool creditsGoBack = false;
    // Start is called before the first frame update
    void Start()
    {   
        EventSystem.current.SetSelectedGameObject(startButton);

        username = PlayerPrefs.GetString("User");
        displayStartMenu = true;
        userPath = Path.Combine(Application.persistentDataPath, $"Users/{username.Split(" ")[0]}/config.user");
        SkinIndex = getIndexValue("SkinIndex");
        DifficultyIndex = getIndexValue("DifficultyIndex");
        BoardIndex = getIndexValue("BoardIndex");
        if (getIndexStringValue("Obstacles") == "yes") {
            obstacleIndex = 0;
        }
        else {
            obstacleIndex = 1;
        }
        ChangeSkin();
        BoardText.text = sizes[BoardIndex];
        DifficultyText.text = options[DifficultyIndex];
        obstacleText.text = obstacleOptions[obstacleIndex];


    }
    public int getIndexValue(string index) {
        string[] lines = File.ReadAllLines(userPath);
        int Index = 0;
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].StartsWith($"{index}: ")) {
                string value = lines[i].Split(':')[1].Trim();
                Index = int.Parse(value);
            }
        }
        return Index;
    }
    public string getIndexStringValue(string index) {
        string value = "";
        string[] lines = File.ReadAllLines(userPath);
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].StartsWith($"{index}: ")) {
                value = lines[i].Split(':')[1].Trim();
            }
        }
        return value;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayStartMenu)
        {
            StartMenu.SetActive(true);
            settings.SetActive(false);
        }
        else
        {
            StartMenu.SetActive(false);
        }

    }
    public void StartGameButton()
    {
        Debug.Log("pressed!");
        PlayerPrefs.SetInt("SkinIndex", SkinIndex);
        PlayerPrefs.SetInt("DifficultyIndex", DifficultyIndex);
        PlayerPrefs.SetInt("BoardIndex", BoardIndex);
        
        
        if (obstacleIndex == 0) {
            PlayerPrefs.SetString("Obstacles", "yes");
        }
        else {
            PlayerPrefs.SetString("Obstacles", "no");
        }
        string isObstacle = PlayerPrefs.GetString("Obstacles"); 
        string[] lines = File.ReadAllLines(userPath);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("SkinIndex:"))
            {
                lines[i] = $"SkinIndex: {SkinIndex}";
                Debug.Log("starts with at least?");
            }

            if (lines[i].StartsWith("DifficultyIndex:"))
            {
                lines[i] = $"DifficultyIndex: {DifficultyIndex}";
            }

            if (lines[i].StartsWith("BoardIndex:"))
            {
                lines[i] = $"BoardIndex: {BoardIndex}";
            }

            if (lines[i].StartsWith("Obstacles:"))
            {
                lines[i] = $"Obstacles: {isObstacle}";
            }
        }
        File.WriteAllLines(userPath, lines);

        GameObject.Find("Sound Manager").GetComponent<SoundManager>().PlaySound(GameObject.Find("Sound Manager").GetComponent<SoundManager>().buttonClickSound);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

    }

    public void SettingsButton()
    {
        displayStartMenu = false;
        settings.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);

        StartCoroutine(DisplaySettings());



    }
    public void RightDifficultyButton() {

        DifficultyIndex = (DifficultyIndex + 1) % options.Length;
        DifficultyText.text = options[DifficultyIndex];



    }
    public void LeftDifficultyButton() {

        DifficultyIndex = (DifficultyIndex - 1 + options.Length) % options.Length;
        DifficultyText.text = options[DifficultyIndex];

        

    }

    public void RightSkinButton() {
        SkinIndex = (SkinIndex + 1) % skinOptions.Length;
        ChangeSkin();
        
    }
    public void LeftSkinButton() {
        SkinIndex = (SkinIndex - 1 + skinOptions.Length) % skinOptions.Length;
        ChangeSkin();
    }
    public void ChangeSkin() {
        for (int i = 0; i < skins.Length; i++) {
            skins[i].SetActive(false);
        }

        skins[SkinIndex].SetActive(true);

    }
    public void CreditsButton() { 
        credits.SetActive(true);
        displayStartMenu = false;
        StartCoroutine(DisplayCredits());


    }
    public void RightBoardButton() {
        BoardIndex = (BoardIndex + 1) % sizes.Length;
        BoardText.text = sizes[BoardIndex];

    }
    public void LeftBoardButton() {
        BoardIndex = (BoardIndex - 1 + sizes.Length) % sizes.Length;
        BoardText.text = sizes[BoardIndex];
    }
    public void RightObstaclesButton() {
        obstacleIndex = (obstacleIndex + 1 + obstacleOptions.Length) % obstacleOptions.Length;
        obstacleText.text = obstacleOptions[obstacleIndex];
    }
    public void LeftObstaclesButton() {
        obstacleIndex = (obstacleIndex - 1 + obstacleOptions.Length) % obstacleOptions.Length;
        obstacleText.text = obstacleOptions[obstacleIndex];
    }
    IEnumerator DisplayCredits() {
        while (!creditsGoBack) {
            yield return null;
        }
        credits.SetActive(false);
        displayStartMenu = true;
        creditsGoBack = false;
        EventSystem.current.SetSelectedGameObject(startButton);

    }
    IEnumerator DisplaySettings() {
        EventSystem.current.SetSelectedGameObject(LeftArrow);

        while (!settingsGoBack) {
            yield return null;
        }
        settings.SetActive(false);
        displayStartMenu = true;
        settingsGoBack = false;
        EventSystem.current.SetSelectedGameObject(startButton);

    }
    public void BackUser() {
        SceneManager.LoadScene("TypeofGame", LoadSceneMode.Single);
    }
    public void SettingsBackButton() {
        settingsGoBack = true;
    }
    public void CreditsBackButton() {
        creditsGoBack = true;
    }

}

