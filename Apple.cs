using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public snakeManager snakeManager;
    // Start is called before the first frame update
    void Start()
    {
        snakeManager = GameObject.Find("snake manager").GetComponent<snakeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
