using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DestroyUserDropdown : MonoBehaviour
{


    private int userSelected;
    public Button button;
    public GameObject confirmPopUp;
    public Button confirmButton;
    public Button cancelButton;
    public GameObject selectUserButton;
    public DropdownUser userSelectionScript;
    // replace with a button; "delete user"  done
    // it should take the currently selected user from the dropdown list done
    // if no user is selected, the button should be either greyed out or it shouldn't work x
    // when you delete a user, it should ask for confirmation (yes/no) done
    void Start() {
        button.GetComponent<Button>().interactable = false;
        confirmPopUp.SetActive(false);
        userSelectionScript = selectUserButton.GetComponent<DropdownUser>();



    }
    void Update() {
        // make it so it becomes inactive upon deselect
        if (!(userSelectionScript.selectedUser == "")) {
            button.GetComponent<Button>().interactable = true;
            Debug.Log($"this is the selected user: {userSelectionScript.selectedUser}");

        }
        else {
            button.GetComponent<Button>().interactable = false;

        }

    }


    public void ConfirmingButton() {
            confirmPopUp.SetActive(true);
    }
    public void finalConfirmButton() {
        string selectedUser = userSelectionScript.selectedUser; ;
        string filePath = Path.Combine(Application.persistentDataPath, $"Users/{selectedUser.Split('-')[0].Trim()}");
        Directory.Delete(filePath, true);
        userSelectionScript.listOfUsers();
        confirmPopUp.SetActive(false);
    }
    public void cancellingButton() {
        confirmPopUp.SetActive(false);

    }
    
}


