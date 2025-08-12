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

public class snakeManager : MonoBehaviour
{
    public GameObject[] colorPrefabsParts;
    public GameObject[] colorPrefabsHeads;


    // the instantiated snake head
    private GameObject snakeHeadObject;

    // if true, then the game is actively playing, if false, the game is not playing
    public bool isGameRunning = false;

    // starting length of the snake
    public int StartingSnakeLength = 2;

    // constant object describing the length of a grid square
    public const float GridSize = 10.6f;

    // Enums for DirectionX and DirectionY
    public enum DirectionX { Right = 1, Left = -1 }
    public enum DirectionY { Up = 1, Down = -1 }

    // screen that displays Game Over
    public GameObject GameOverScreen;

    // whether or not the game has begun yet
    public bool HasGameStarted = false;

    // the starting position of the head
    private Vector3 startingHeadPosition = new Vector3(-14.77f, 5.66f, 0.2f);

    // the Snake's speed (how often it moves, i.e. 0.4 = every 0.4sec)
    public float Speed = 0.6f;
    
    // Dimensions of game screen
    public  int gridSizeX = 20;
    public  int gridSizeY = 17;

    // Range where apple can spawn
    public  float AppleXRange = -100.4f;
    public  float AppleYRange = 90.5f;

    // Current X and Y of the apple
    public float AppleX;
    public float AppleY;

    // the Prefab of the apple
    public GameObject ApplePrefab;

    // The gameobject of the apple
    public GameObject Apple;

    // Current length of the snake
    public int Length = 1;

    // name of the last part
    public string LastPart;


    public bool snakeGrow = false;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public bool StartingSequenceUp;
    private bool displayStartMenu = false;
    public GameObject StartMenu;


    public int AppleFiveCounter = 0;
    public int CurrentFrame;
    public bool doneGameOver = false;
    public bool startedGameOverSequence = false;
    public string line2 = "";
    string LogFilePath;
    string home = Environment.GetEnvironmentVariable("HOME");
    private int SkinIndex = 0;

    public enum HeadRotation {Right = 0, Left = 180, Down = 90, Up = -90}

    public GameObject bigBoard;
    public GameObject mediumBoard;
    public GameObject smallBoard;

    public List<(float, float)> obstacleLocations = new List<(float, float)>();
    public GameObject obstaclePrefab;
    private int obstacleAmount = 10;
    private float og_speed;
    private int speedBoostTimer = 0;

    private int currentLevel = 1;
    public GameObject levelCompleteScreen;
    public TextMeshProUGUI levelText;
    public bool gamePaused = false;
    public GameObject victoryScreen;
    public TextMeshProUGUI finalScoreText;
    public GameObject countdownScreen;
    public TextMeshProUGUI countdownText;
    private float countdownTime = 3;
    public bool AppleAte = false;
    public bool displayCountdown = false;
    public Joystick joystick;
    public bool RightPressed;
    public bool LeftPressed;
    public bool UpPressed;
    public bool DownPressed;
    void Start()
    {
        StartingSequence();
    }
    void Countdown() {
        if (countdownTime > 0) {
            countdownTime -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdownTime).ToString();
        }
        else {
            countdownText.text = "GO!";
            HasGameStarted = true;
            isGameRunning = true;
            displayCountdown = false;
            StartCoroutine(AppleBlink());
            gamePaused = false;
            countdownScreen.SetActive(false);
            countdownTime = 3;
            snakeHeadObject.GetComponent<snakeHead>().directionX = DirectionX.Right; 

        }
    }
    void StartingSequence()
    {
        displayCountdown = true;
        og_speed = Speed;
        countdownScreen.SetActive(true);
        LogFilePath = @$"{home}/.local/snake/logs/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss")+ ".log \n";
        scoreText.gameObject.SetActive(true);
        speedText.gameObject.SetActive(true);
        Speed /= PlayerPrefs.GetInt("DifficultyIndex")+1;

        SkinIndex = PlayerPrefs.GetInt("SkinIndex");
        if (PlayerPrefs.GetInt("BoardIndex") == 0) {
            smallBoard.SetActive(true);
            gridSizeX = 19;
            gridSizeY = 9;
            
            AppleYRange = 4;
            AppleXRange = -79.1f;

        }
        if (PlayerPrefs.GetInt("BoardIndex") == 1) {
            mediumBoard.SetActive(true);
            gridSizeX = 20;
            gridSizeY = 17;

        }
        if (PlayerPrefs.GetInt("BoardIndex") == 2) {
            bigBoard.SetActive(true);
            gridSizeX = 20;
            gridSizeY = 19;
            AppleYRange = 109.06f;
        }
        if (PlayerPrefs.GetString("Obstacles") == "yes" && PlayerPrefs.GetString("isStoryMode") == "False") {
            for (int i = 0; i < obstacleAmount; i++)  {
                GameObject o = Instantiate(obstaclePrefab);
                o.gameObject.transform.position = ObstaclePosition();
            }

        }
        snakeHeadObject = Instantiate(colorPrefabsHeads[SkinIndex]);
        snakeHeadObject.gameObject.transform.position = startingHeadPosition;
        snakeHeadObject.name = "Head";

        // instantiates the Snake Parts behind the Head GameObject and names them appropriately
        for (int snakePartLoop = 1; snakePartLoop < StartingSnakeLength; snakePartLoop++)
        {

            GameObject snakePart = Instantiate(colorPrefabsParts[SkinIndex]);
            Vector3 startingPartOffset = new Vector3(-snakePartLoop * GridSize, 0, 0);
            snakePart.transform.position = startingHeadPosition + startingPartOffset;
            snakePart.name = $"{snakePartLoop}";
            Length += 1;
            LastPart = snakePart.name;

        }

        // spawns the apple
        Apple = Instantiate(ApplePrefab);
        Apple.name = "Apple";
        Apple.transform.position = MoveApple();
    }

    void Reset() {
        for (int partIndex = 1; partIndex < Length; partIndex++)
        {

            GameObject snakeP = GameObject.Find($"{partIndex}");
            Destroy(snakeP);
        }
        Length = 1;
        Destroy(GameObject.Find("Head"));

        snakeHeadObject = Instantiate(colorPrefabsHeads[SkinIndex]);
        snakeHeadObject.gameObject.transform.position = startingHeadPosition;
        snakeHeadObject.name = "Head";
        // snakeHeadObject.GetComponent<snakeHead>().eyes = GameObject.Find("Eyes");
        snakeHeadObject.GetComponent<snakeHead>().directionX = DirectionX.Right;

        for (int snakePartLoop = 1; snakePartLoop < StartingSnakeLength; snakePartLoop++)
        {
            GameObject snakePart = Instantiate(colorPrefabsParts[SkinIndex]);
            Vector3 startingPartOffset = new Vector3(-snakePartLoop * GridSize, 0, 0);
            snakePart.transform.position = startingHeadPosition + startingPartOffset;
            snakePart.name = $"{snakePartLoop}";
            Length += 1;
            LastPart = snakePart.name;
        }
        
        }

    void Update()

    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            UpPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            DownPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            RightPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            LeftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            UpPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            DownPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            RightPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            LeftPressed = false;
        }
        scoreText.text = "Score: " + score;
        speedText.text = "Speed: " + Mathf.RoundToInt(og_speed*10);

        if (countdownScreen.activeSelf == true && displayCountdown == true) {
            Countdown();
        }
        // if the game is no longer running and the game has already started, display the Game Over screen
        if (isGameRunning == false && HasGameStarted == true)
        {
            GameOverScreen.SetActive(true);
            string pathway = Path.Combine(Application.persistentDataPath, $"Users/{PlayerPrefs.GetString("User").Split(" ")[0]}/config.user");
            Directory.CreateDirectory(Path.GetDirectoryName(pathway)); 
            string[] lines = File.ReadAllLines(pathway);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("BestScore:"))
                {
                    if (lines[i].Split(':')[1].Trim() != "") {
                        int pastScore = int.Parse(lines[i].Split(':')[1].Trim());
                        if (score > pastScore) {
                            lines[i] = $"BestScore: {score}";

                        }
                    }
                    else {
                        lines[i] = $"BestScore: {score}";

                    }
                }
            }
            File.WriteAllLines(pathway, lines);

            if (startedGameOverSequence == false) {
                StartCoroutine(WaitOnGameOver());
                
                startedGameOverSequence = true;
                
            }
            if (doneGameOver) {
                RestartGame();
                doneGameOver = false;
            }

            
        }
       
        if (!Apple.activeInHierarchy && AppleAte == true)
        {
            Apple.transform.position = MoveApple();
            Apple.SetActive(true);
            AppleAte = false;
        }
        if (AppleFiveCounter == 5) {
            AppleFiveCounter = 0;
            Speed -= og_speed / 5;
            og_speed = Speed;
            if (PlayerPrefs.GetString("isStoryMode") == "True" && currentLevel < 11)
            {
                currentLevel += 1;
                gamePaused = true;
                StartCoroutine(displayLevelWait());


                Reset();
                
                if (PlayerPrefs.GetInt("BoardIndex") == 0)
                {
                    createObstacles(5);
                }
                if (PlayerPrefs.GetInt("BoardIndex") == 1)
                {
                    createObstacles(6);
                }
                if (PlayerPrefs.GetInt("BoardIndex") == 2)
                {
                    createObstacles(7);
                }

            }
            else if (currentLevel > 10)
            {
                StartCoroutine(displayLevelWait());
                RestartGame();
            }
        }
        if (isGameRunning)
        {
            if (LeftPressed || RightPressed || DownPressed || UpPressed)
            {
                Debug.Log(speedBoostTimer);
                speedBoostTimer += 1;
                if (speedBoostTimer >= 70)
                {
                    Speed = og_speed / 3;
                }
                else {
                    og_speed = Speed;
                }
            }
        }
        if (LeftPressed == false && RightPressed == false && DownPressed == false && UpPressed == false) {
            Speed = og_speed;
            speedBoostTimer = 0;
        }
        CurrentFrame += 1;
        WriteLog();
        

    }
    public void createObstacles(int obstacleAmount)
    {
        Debug.Log("??????");
        for (int i = 0; i < obstacleAmount; i++)
        {
            GameObject o = Instantiate(obstaclePrefab);
            o.gameObject.transform.position = ObstaclePosition();
        }
        
    }
    IEnumerator AppleBlink() {
        while (isGameRunning)
        {

            if (isGameRunning == true)
            {
                if (Apple.activeSelf == false)
                {
                    Apple.SetActive(true);
                }
                else
                {
                    Apple.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }
    IEnumerator displayVictory() {
        victoryScreen.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";
        while (!Input.anyKeyDown) {
                    yield return null;
                    }
        victoryScreen.SetActive(false);
        RestartGame();
    }
    IEnumerator displayLevelWait() {
        levelCompleteScreen.SetActive(true);
        levelText.text = $"Level {currentLevel} Complete";
        while (!Input.anyKeyDown) {
                    yield return null;
                }
        levelCompleteScreen.SetActive(false);
        displayCountdown = true;
        countdownScreen.SetActive(true);


    }
    // writes the log
    public void WriteLog() {
        
        if (!File.Exists(@$"{home}/.local/snake/logs/")) {
            Directory.CreateDirectory(@$"{home}/.local/snake/logs/");	
        }
        if (!File.Exists(LogFilePath))
        {
            File.Create(LogFilePath).Dispose();
        }
        
        var LineINFO = new Dictionary<string, object>
        {
            { "SnakeHeadX", snakeHeadObject.transform.position.x },
            { "SnakeHeadY", snakeHeadObject.transform.position.y },
            { "SnakeHeadPrevX", snakeHeadObject.GetComponent<snakeHead>().prev_coordX },
            { "SnakeHeadPrevY", snakeHeadObject.GetComponent<snakeHead>().prev_coordY },
            { "IsGameRunning", isGameRunning },
            { "hasGameStarted", HasGameStarted },
            { "speed", Speed },
            { "appleX", AppleX },
            { "appleY", AppleY },
            { "length", Length },
            { "HeadDirectionX", snakeHeadObject.GetComponent<snakeHead>().directionX },
            { "HeadDirectionY", snakeHeadObject.GetComponent<snakeHead>().directionY }
        };
        for (int i = 1; i < Length; i++) {
                GameObject snakeParty = GameObject.Find($"{i}");
            if (!(snakeParty == null))
            {
                LineINFO[$"SnakePart{i}X"] = snakeParty.transform.position.x;
                LineINFO[$"SnakePart{i}Y"] = snakeParty.transform.position.y;
            }
            }
        string jsonINFO = JsonUtility.ToJson(LineINFO, true);
        string line = $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}: snakeheadX: {LineINFO["SnakeHeadX"]} snakeheadY: {LineINFO["SnakeHeadY"]} snakeHeadPrevX: {LineINFO["SnakeHeadPrevX"]} snakeHeadPrevY: {LineINFO["SnakeHeadPrevY"]} isGameRunning: {LineINFO["IsGameRunning"]} hasGameStarted: {LineINFO["hasGameStarted"]} speed: {LineINFO["speed"]} appleX: {LineINFO["appleX"]} appleY: {LineINFO["appleY"]} length: {LineINFO["length"]} HeadDirectionX: {LineINFO["HeadDirectionX"]} HeadDirectionY: {LineINFO["HeadDirectionY"]} ";
        line2 = "";
        bool startIterating = false;
        foreach (var item in LineINFO)
        {
            if (startIterating)
            {
                line2 += $" {item.Key}: {item.Value}";
            }

            if (item.Key == "HeadDirectionY")
            {
                startIterating = true;
            }
        }
        line += line2;
        line += "\n";
        using (StreamWriter writer = new StreamWriter(LogFilePath, append: true))
        {
            writer.WriteLine(line);
        }
    }
    public void AddToLog(string Addition) {
        using (StreamWriter writer = new StreamWriter(LogFilePath, append: true))
        {
            writer.WriteLine("\n"+Addition);
        }
    }
    // adds a new snake part
    public void AddPart()
    {
        int NewPart = int.Parse(LastPart) + 1;
        GameObject LastPartObject = GameObject.Find(LastPart);
        
        float newX = LastPartObject.GetComponent<snakePart>().prev_coordX;
        float newY = LastPartObject.GetComponent<snakePart>().prev_coordY;

       
        GameObject NewPartObject = Instantiate(colorPrefabsParts[SkinIndex]);

        NewPartObject.transform.position = new Vector3(newX, newY, NewPartObject.transform.position.z);
        Length += 1;
        NewPartObject.name = $"{NewPart}";
        LastPart = $"{NewPart}";

    }
    // changes position of apple
    public Vector3 MoveApple()
    {
        AppleX = Random.Range(1, gridSizeX) * GridSize + AppleXRange;
        AppleY = AppleYRange - Random.Range(1, gridSizeY) * GridSize;
        bool canReturn = false;
        while (!canReturn) {
            if (!obstacleLocations.Contains((AppleX, AppleY))) {
                canReturn = true;
            }
            else {
                canReturn = false;
            } 
            }
        return new Vector3(AppleX, AppleY, -0.1f);

    }
    public Vector3 ObstaclePosition() {
        bool canReturn = false;
        float obstacleX = 0f;
        float obstacleY = 0f;
        while (canReturn == false)
        {
            obstacleX = Random.Range(1, gridSizeX) * GridSize + AppleXRange;
            obstacleY = AppleYRange - Random.Range(1, gridSizeY) * GridSize;
            if (obstacleLocations.Contains((obstacleX, obstacleY)))
            {
                canReturn = false;

            }
            else
            {
                canReturn = true;
                
            }
        }
        obstacleLocations.Add((obstacleX, obstacleY));
        if (PlayerPrefs.GetInt("BoardIndex") == 0)
        {
            return new Vector3(obstacleX, obstacleY, -0.2f) + new Vector3(-0.3f, 1.4f);
        }
        else if (PlayerPrefs.GetInt("BoardIndex") == 1) {
            return new Vector3(obstacleX, obstacleY, -0.2f);
        }
        else {
            return new Vector3(obstacleX, obstacleY, -0.2f);
        }
    }

    // waits until user presses a key to go back to start menu at death
    IEnumerator WaitOnGameOver() {
        while (!Input.anyKeyDown) {
            yield return null;
        }
        doneGameOver = true;


    }
    
    public void RestartGame() {
        SceneManager.LoadScene("TypeofGame");
    }

    // public void UpArrow() {
    //     UpPressed = true;
    // }
    // public void DownArrow() {
    //     DownPressed = true;
    // }
    // public void LeftArrow() {
    //     LeftPressed = true;
    // }
    // public void RightArrow() {
    //     RightPressed = true;
    // }
}
