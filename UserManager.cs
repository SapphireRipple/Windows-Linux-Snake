using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
public class UserManager : MonoBehaviour
{
    public GameObject UserSelectionMenu;
    public GameObject CreateUserMenu;
    public TMP_Dropdown UserSelection;
    public GameObject information_popup;
    public GameObject createUserButton;
    public GameObject inputUser;

    // Start is called before the first frame update
    void Start()
    {
        UserSelectionMenu.SetActive(true);
        CreateUserMenu.SetActive(false);
        information_popup.SetActive(false);
        EventSystem.current.SetSelectedGameObject(createUserButton);

    }
    
    // Update is called once per frame
    void Update()
    {
        // if (Directory.GetDirectories("Users").Length > 0)
        // {
        //     UserSelection.gameObject.SetActive(true);
        // }
        // else {
        //     UserSelection.gameObject.SetActive(false);
        // }
    }
    public void CreateUser() {
        CreateUserMenu.SetActive(true);
        UserSelectionMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(inputUser);

        StartCoroutine(returnToUserScreen());
    }
    public void goToUserScreen() {
        CreateUserMenu.SetActive(false);
        UserSelectionMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(createUserButton);

    }
    public void displayInformation() {
        information_popup.SetActive(true);
        StartCoroutine(informationCounter());
    }
    IEnumerator informationCounter() {
        while (!Input.GetKeyDown(KeyCode.Escape)) {
            yield return null;
        }
        information_popup.SetActive(false);
    }
    IEnumerator returnToUserScreen() {
        while (!Input.GetKeyDown(KeyCode.Escape)) {
            yield return null;
        }
        goToUserScreen();
    }
    
}
