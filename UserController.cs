using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class UserController : MonoBehaviour
{   
    [SerializeField] private string inputText;
    string path = "Users";
    public GameObject userWarning1;
    public GameObject userWarning2;
    public GameObject userWarning3;
    public GameObject selectUserButton;
    public DropdownUser userSelectionScript;
    public TMP_InputField inputUser;
    public GameObject userManager;
    private UserManager userManagerScript;
    public GameObject okPopUp;
    public Button confirmButton;
    private string[] filteredWords;
    public GameObject createUserButton;
    // to do: change the structure of the user files. every user should have their own folder, with a file "config.user" inside done 
    // the structure of the file can stay the same done
    // change in menumanager the config.user done
    // inputText should exclude all special symbols (for now) done
    // set a limit to the amount of characters (make it 10) done
    // once a new user is created, it should trigger a function "listallusers" to recreate the dropdown list done

    // to do:
    // do not show the previously created user done
    // add a button "create" user (there should be two methods, press the button or press enter) done
    // escape should return you to the previous menu done
    // pressing enter should finish the sequence of user creation and user should be returned to the previous menu done
    // it should also display that the user was created successfully done
    // fix the fact that there's no message displaying "user select" originally done
    // return to user selection by pressing escape and a button "return to user selection" done
    // make a button named "start" underneath the user selection, and also use enter when you select a user done
    // change background, select a more modern font done
    // font size/boldness should mean something; like buttons should have exactly the same font, size, thickness etc done
    // color of button can change done 
    // messages should be the same font, but can be bigger or smaller, depends on effect done
    // add a frame or small description of the functionality of the menu done
    // pressing escape gives error
    // check all scripts for updates every frame, and it's not a gameobject
    void Start() {

        userWarning2.SetActive(false);
        userWarning3.SetActive(false);
        okPopUp.SetActive(false);
        userManagerScript = userManager.GetComponent<UserManager>();
        userSelectionScript = selectUserButton.GetComponent<DropdownUser>();
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        filteredWords = File.ReadAllLines(Path.Combine(Application.persistentDataPath, "filter.txt"));

    }
    public void confirmationButton() {
        SubmitUsername(inputUser.text);
    }
    public void SubmitUsername(string input)
    {
        inputText = input;
        if (true)
        {
            if (inputText.Length > 1)
            {
                string pathway = Path.Combine(Application.persistentDataPath, $"{path}/{inputText}");  
                string configPathway = Path.Combine(Application.persistentDataPath, $"{path}/{inputText}/config.user"); 

                if (!Directory.Exists(pathway))
                {
                    Directory.CreateDirectory(pathway);
                    File.WriteAllLines(configPathway, new string[]
                    {
                    $"SkinIndex: 0",
                    $"DifficultyIndex: 0",
                    $"BoardIndex: 0",
                    $"Obstacles: no",
                    $"BestScore: 0"

                    });
                    userSelectionScript.listOfUsers();
                    okPopUp.SetActive(true);
                    StartCoroutine(returnfromOkPopUp());
                }
                else
                {
                    if (!userWarning2.activeSelf && !userWarning3.activeSelf)
                    {
                        userWarning1.SetActive(true);
                        StartCoroutine(displayWarnings(userWarning1));
                    }
                }
            }

            else
            {
                if (!userWarning3.activeSelf && !userWarning1.activeSelf)
                {
                    userWarning2.SetActive(true);
                    StartCoroutine(displayWarnings(userWarning2));
                }
            }

        }
        // else {
        //     if (!userWarning2.activeSelf && !userWarning1.activeSelf)
        //     {
        //         userWarning3.SetActive(true);
        //         StartCoroutine(displayWarnings(userWarning3));
        //     }
        // }
    inputUser.text = "";

    }
    public void okButton() {
        okPopUp.SetActive(false);
        userManagerScript.goToUserScreen();

    }
    private IEnumerator returnfromOkPopUp() {
        while (!Input.GetKeyDown(KeyCode.Escape)) {
            yield return null;
        }
        if (okPopUp.activeSelf) {
            okPopUp.SetActive(false);
        }
        
    }
    public void ReturnToMainScreen() {
        userManagerScript.goToUserScreen();

    }
    IEnumerator displayWarnings(GameObject warning) {
        int i = 0;
        while (i < 4)
        {
            i += 1;
            yield return new WaitForSeconds(0.4f);
        }
        warning.SetActive(false);
    }
    
}
