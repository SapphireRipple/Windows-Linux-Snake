using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class modeManager : MonoBehaviour
{
    public GameObject defaultButton;
 
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton);

    }

    public void BackButton() {
        SceneManager.LoadScene("User Selection", LoadSceneMode.Single);

    }
    public void sandBoxMode() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void storyMode() {
        PlayerPrefs.SetString("isStoryMode", "True");
        PlayerPrefs.SetInt("DifficultyIndex", 0);
        PlayerPrefs.SetInt("BoardIndex", 1);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);



    }
}
