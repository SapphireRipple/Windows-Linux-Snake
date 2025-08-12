using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class snakeHead : MonoBehaviour
{
    public snakeManager snakeManager;

    // previous X and Y coordinates
    public float prev_coordX;
    public float prev_coordY;

    // whether or not the snake has begun to move, (the Coroutine has begun)
    private bool startedMoving = false;

    // snake head's current direction
    public snakeManager.DirectionX directionX;
    public snakeManager.DirectionY directionY;
    public snakeManager.DirectionX canMoveDirectionX;
    public snakeManager.DirectionY canMoveDirectionY;

    Dictionary<string, float> BoardDimensions = new Dictionary<string, float>();
    private float RightSideX = 111.2f;
    private float LeftSideX = -100.8f;
    private float TopSideY = 90.2f;
    private float BottomSideY = -90 + snakeManager.GridSize;

    private int counter = 0;
    bool isTouchingWall;
    bool startedCoroutine = false;
    Vector2Int direction;
    int wallWaitFrames = 20;
    int currentWaitFrames = 0;
    bool waitingAtWall = false;
    Vector3 nextPosition;
    long timeTillAppleEat;
    public GameObject eyes;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("BoardIndex") == 1) {
            BoardDimensions.Add("RightSideX", 112.43f);
            BoardDimensions.Add("LeftSideX", -99.56999f);
            BoardDimensions.Add("TopSideY", 90.45999f);
            BoardDimensions.Add("BottomSideY", -79.14f);
        }
        if (PlayerPrefs.GetInt("BoardIndex") == 0) {
            BoardDimensions.Add("TopSideY", 5.66f);
            BoardDimensions.Add("BottomSideY", -79.14f);
            BoardDimensions.Add("LeftSideX", -78.37f);
            BoardDimensions.Add("RightSideX", 112.43f);
        }
        if (PlayerPrefs.GetInt("BoardIndex") == 2) {
            BoardDimensions.Add("RightSideX", 112.43f);
            BoardDimensions.Add("LeftSideX", -99.56999f);
            BoardDimensions.Add("TopSideY", 111.66f);
            BoardDimensions.Add("BottomSideY", -79.14f);
        }
        snakeManager = GameObject.Find("snake manager").GetComponent<snakeManager>();
        eyes = GameObject.Find("Eyes");
        timeTillAppleEat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    IEnumerator Move()
    {

        while (snakeManager.isGameRunning)
        {
            eyes = GameObject.Find("Eyes");

            if (snakeManager.isGameRunning && snakeManager.gamePaused == false)
            {
                nextPosition = transform.position + new Vector3(snakeManager.GridSize * (int)directionX, snakeManager.GridSize * (int)directionY, 0);

                if (isPositionAtWall(nextPosition))
                {
                    int wallWaitFrames = 10;
                    for (int i = 0; i < wallWaitFrames; i++)
                    {
                        if (directionX != 0 || directionY != 0)
                            break;

                        yield return new WaitForSeconds(snakeManager.Speed / 4);
                    }
                    if (isPositionAtWall(transform.position + new Vector3(snakeManager.GridSize * (int)directionX, snakeManager.GridSize * (int)directionY, 0)))
                    {
                        snakeManager.isGameRunning = false;
                        yield break;

                    }
                }

                if (!(directionY == 0)) 
                {
                    transform.position += new Vector3(0, snakeManager.GridSize * (int)directionY, 0);
                }


                if (!(directionX == 0))
                {
                    transform.position += new Vector3(snakeManager.GridSize * (int)directionX, 0, 0);

                }

                for (int partIndex = 1; partIndex < snakeManager.Length; partIndex++)
                {
                    Debug.Log(snakeManager.Length);
                    GameObject snakeP = GameObject.Find($"{partIndex}");
                    snakeP.GetComponent<snakePart>().prev_coordX = snakeP.transform.position.x;
                    snakeP.GetComponent<snakePart>().prev_coordY = snakeP.transform.position.y;
                    if (snakeP.GetComponent<snakePart>().PrevPart.name == "Head")
                    {
                        snakeP.transform.position = new Vector3(prev_coordX, prev_coordY, snakeP.transform.position.z);
                        

                    }
                    else        

                    {
                        snakeP.transform.position = new Vector3(snakeP.GetComponent<snakePart>().PrevPart.GetComponent<snakePart>().prev_coordX, snakeP.GetComponent<snakePart>().PrevPart.GetComponent<snakePart>().prev_coordY, snakeP.transform.position.z);
                    }

                }

                if (snakeManager.snakeGrow)
                {
                    double timeDifference = Math.Round((double)DateTimeOffset.UtcNow.ToUnixTimeSeconds()- timeTillAppleEat);
                    GameObject.Find("Sound Manager").GetComponent<SoundManager>().PlaySound(GameObject.Find("Sound Manager").GetComponent<SoundManager>().appleEat);
                    if (PlayerPrefs.GetString("isStoryMode") == "True") {
                        snakeManager.score += returnScore(timeDifference, 5, 13, 5, 10);
                    }
                    else if (PlayerPrefs.GetString("isStoryMode") == "False") {
                        snakeManager.score += 1;
                    }
                    else {
                        snakeManager.score += 5;
                    }
                    snakeManager.AddPart();
                    snakeManager.AppleFiveCounter += 1;
                    snakeManager.snakeGrow = false;
                    timeTillAppleEat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                }
            }
            yield return new WaitForSeconds(snakeManager.Speed);


        }

    }
    int returnScore(double time, int minTimeLimit, int maxTimeLimit, int minPoints, int maxPoints) {
        int points = 0;
        if (time < maxTimeLimit) {
            double reduction = Math.Truncate((time - minTimeLimit) / 2);
            if (reduction > 0) {
                points = maxPoints - (int)reduction; }
            else {
                points = maxPoints;
            }
        }
        else {
            points = minPoints;
            }
        return points;
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        // stops the game upon colliding
        if (collider.name == "Apple")
        {
            collider.gameObject.SetActive(false);
            snakeManager.snakeGrow = true;
            snakeManager.AppleAte = true;

        }
        else
        {
            Debug.Log("collision death!");
            snakeManager.isGameRunning = false;
        }

    }




    bool isPositionAtWall(Vector2 nextPos)
    {
        isTouchingWall = Mathf.Approximately(nextPos.y, BoardDimensions["TopSideY"] + snakeManager.GridSize) ||
            Mathf.Approximately(nextPos.y, BoardDimensions["BottomSideY"] - snakeManager.GridSize) ||
            Mathf.Approximately(nextPos.x, BoardDimensions["RightSideX"] + snakeManager.GridSize) ||
            Mathf.Approximately(nextPos.x, BoardDimensions["LeftSideX"] - snakeManager.GridSize);
        return isTouchingWall;
    }
    void Update()
    {
        Debug.Log(eyes);
        // float gridSize = snakeManager.GridSize;
        prev_coordX = gameObject.transform.position.x;
        prev_coordY = gameObject.transform.position.y;
        if (snakeManager.LeftPressed && directionX != snakeManager.DirectionX.Right)
        {
            directionX = snakeManager.DirectionX.Left;
            directionY = 0;
            if (!(eyes == null))
            {

                eyes.transform.rotation = Quaternion.Euler(0f, (float)snakeManager.HeadRotation.Left, 0f) * Quaternion.Euler(-270f, 0, 0);
            }
            Debug.Log("this is left");
        }
        else if (snakeManager.RightPressed && directionX != snakeManager.DirectionX.Left)
        {
            directionX = snakeManager.DirectionX.Right;
            directionY = 0;
            if (!(eyes == null))
            {
                eyes.transform.rotation = Quaternion.Euler(0f, (float)snakeManager.HeadRotation.Right, 0f) * Quaternion.Euler(-90f, 0, 0);
            }
            Debug.Log("this is right");

        }
    
    

        // updates direction based on which key is pressed
        if (snakeManager.UpPressed && directionY != snakeManager.DirectionY.Down)
        {
            directionY = snakeManager.DirectionY.Up;
            directionX = 0;
            if (!(eyes == null))
            {

                eyes.transform.rotation = Quaternion.Euler(0f, (float)snakeManager.HeadRotation.Up, 0f) * Quaternion.Euler(0f, 0f, 90f);
            }
            Debug.Log("this is up");

        }
        else if (snakeManager.DownPressed && directionY != snakeManager.DirectionY.Up)
        {
            directionY = snakeManager.DirectionY.Down;
            directionX = 0;
            if (!(eyes == null))
            {
                eyes.transform.rotation = Quaternion.Euler(0f, (float)snakeManager.HeadRotation.Down, 0f) * Quaternion.Euler(0, 0, -90f);
            }
            Debug.Log("this is down");

        }

        
        





        // Starts moving when game starts running
        if (startedMoving == false && snakeManager.isGameRunning == true)
        {

            StartCoroutine(Move());
            startedMoving = true;

        }




    }


}

// Open snakeHead.cs
// The file must be open
//
// Find all of the border coordinates by taking the current ones in the if statement for collision at line 74 and subtracting snakeManager.GridSize from all of them
// in the snakeHead.cs script, there must be four variables for all four borders with those coordinates, for example,: int RightSideX = 50; define variables for the borders and assign the coordinates you calculated to them inside of the snakeHead class 
//
// inside of the SnakeHead Update method, there should be logic that determines the following: 
// - whether or not the snake is at the border using coordinates from the previous task
// - using directionX and directionY and coordinates of the snake head, if pressed button matches the pattern necessary to avoid collision, otherwise it kills the snake
// there should be a time-out before the snake dies to allow the user to move and avoid dying