using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class snakePart : MonoBehaviour
{

    // the Snake Head GameObject
    public GameObject SnakeHeadObject;

    // the Snake Head Script
    public snakeHead snakeHeadScript;

    // Previous coordinates X and Y
    public float prev_coordX;
    public float prev_coordY;

    // The part before this one
    public GameObject PrevPart;

    // Snake Manager 
    public GameObject snakeManage;

    // Snake Manager Script
    public snakeManager snakeManager;

    // whether or not the part started moving
    private bool startedMoving = false;

    private bool foundHead = false;
    // Start is called before the first frame update
    void Start()
    {
        // finds snake manager script and snake head script
        snakeManage = GameObject.Find("snake manager");
        snakeManager = snakeManage.GetComponent<snakeManager>();

        SnakeHeadObject = GameObject.Find("Head");
        snakeHeadScript = SnakeHeadObject.GetComponent<snakeHead>();

    }
    // IEnumerator Move() {

    //     while (snakeManager.isGameRunning)
    //     {

    //     prev_coordX = gameObject.transform.position.x;
    //     prev_coordY = gameObject.transform.position.y;


    //         if (snakeManager.isGameRunning)
    //         {
    //             if (PrevPart.name == "Head")
    //             {
    //                 gameObject.transform.position = new Vector3(SnakeHeadObject.GetComponent<snakeHead>().prev_coordX, SnakeHeadObject.GetComponent<snakeHead>().prev_coordY, gameObject.transform.position.z);

    //             }
    //             else
    //             {
    //                 gameObject.transform.position = new Vector3(PrevPart.GetComponent<snakePart>().prev_coordX, PrevPart.GetComponent<snakePart>().prev_coordY, gameObject.transform.position.z);
    //             }
    //         yield return new WaitForSeconds(snakeManager.moveInterval);



    //         }
    //     }
    // }
    // void OnTriggerEnter(Collider collider) {
    //     // detects collision and stops the game accordingly
    //     snakeManager.isGameRunning = false;

    // }
    // Update is called once per frame
    void Update()
    {
        if (!foundHead)
        {
            if (gameObject.name == "1")
            {
                PrevPart = GameObject.FindGameObjectWithTag("Head");
                foundHead = true;
            }
            if (!(gameObject.name == "1"))
            {
                int objectName = int.Parse(gameObject.name);
                objectName = objectName - 1;
                PrevPart = GameObject.Find($"{objectName}");
                foundHead = true;
            }
        }
        // if (startedMoving == false && snakeManager.isGameRunning) {
        //     StartCoroutine(Move());
        //     startedMoving = true;
        // }
        }



    }


