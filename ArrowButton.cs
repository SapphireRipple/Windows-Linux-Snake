using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public snakeManager snakeManager;

    void Awake() {
        snakeManager = GameObject.Find("snake manager").GetComponent<snakeManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("recognized press");
        if (gameObject.name == "Right Arrow") {
            snakeManager.RightPressed = true;
            Debug.Log("right arrow");
        }
        else if (gameObject.name == "Left Arrow") {
            snakeManager.LeftPressed = true;
        }
        else if (gameObject.name == "Down Arrow") {
            snakeManager.DownPressed = true;
        }
        else if (gameObject.name == "Up Arrow") {
            snakeManager.UpPressed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (gameObject.name == "Right Arrow") {
            snakeManager.RightPressed = false;
        }
        else if (gameObject.name == "Left Arrow") {
            snakeManager.LeftPressed = false;
        }
        else if (gameObject.name == "Down Arrow") {
            snakeManager.DownPressed = false;
        }
        else if (gameObject.name == "Up Arrow") {
            snakeManager.UpPressed = false;
        }
    }
}
