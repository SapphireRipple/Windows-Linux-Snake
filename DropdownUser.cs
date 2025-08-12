using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
public class DropdownUser : MonoBehaviour
{

    // public TMP_Dropdown UserSelection;
    public List<string> options;
    private int lastIndex;
    private int lastSelectedIndex = -1;
    private float lastClickTime = -1f;
    public float confirmDelay = 0.5f;
    public bool pickedNewIndex = false;
    public Button createUser;
    public List<Button> UserButtons = new List<Button>();
    public GameObject selectUserPopUp;
    public string selectedUser;

    List<string> users = new List<string>();

    void Start()
    {
        for (int i = 0; i < UserButtons.Count; i++)
        {
            int index = i; // capture index for closure
            Button button = UserButtons[index];

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnUserButtonClicked(button));
        }
        listOfUsers();
        selectUserPopUp.SetActive(false);

    }
    void OnUserButtonClicked(Button clickedButton)
    {
        string userText = clickedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        selectedUser = userText.Split('-')[0].Trim();
        ReturnFromPopUp();

    }
    // public void PickUser() {
    //     int pickedEntryIndex = UserSelection.value;
    //     string SelectedOption = UserSelection.options[pickedEntryIndex].text;

    //     lastIndex = pickedEntryIndex;
    //     if (pickedEntryIndex != 0) {pickedNewIndex = true;}
    //     else { pickedNewIndex = false; }


    // }
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     int currentIndex = UserSelection.value;
    //     float currentTime = Time.time;

    //     if (currentIndex == lastSelectedIndex && (currentTime - lastClickTime <= confirmDelay) && currentIndex != 0)
    //     {
    //         StartGame();
    //     }
    //     else
    //     {
    //         lastSelectedIndex = currentIndex;
    //         lastClickTime = currentTime;
    //     }
    // }

    private string getIndexValue(string filePath)
    {
        if (File.Exists(filePath))
        {
            Debug.Log("file exists");
        }
        string[] lines = File.ReadAllLines(filePath);
        string score = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith($"BestScore: "))
            {
                Debug.Log("hiphiphooray");
                score = lines[i].Split(':')[1].Trim();
            }
        }
        return score;
    }
    public void listOfUsers()
    {
        // to do: find a way to clear the dropdown list done
        // UserSelection.options.Clear(); done



        string[] usernamePaths = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, "Users"));
        foreach (string path in usernamePaths)
        {
            Debug.Log(usernamePaths + " " + path);
            string username = Path.GetFileName(path);

            if (!users.Contains(username))
            {
                string pathway = Path.Combine(Application.persistentDataPath, $"Users/{username}/config.user");

                // UserSelection.options.Add(new TMP_Dropdown.OptionData($"{username} - {getIndexValue(pathway)}"));
                // options.Add(userName);
                users.Add($"{username} - {getIndexValue(pathway)}");
            }
            
        }
        foreach (Button button in UserButtons)
        {

            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
            Debug.Log($"{button.GetComponentInChildren<TextMeshProUGUI>().text} silly goose {button}");
        }
        foreach (string user in users)
        {
            Debug.Log(user);

            UserButtons[users.IndexOf(user)].GetComponentInChildren<TextMeshProUGUI>().text = user;
            Debug.Log($"{UserButtons[users.IndexOf(user)].GetComponentInChildren<TextMeshProUGUI>().text} please");
        }
        foreach (Button button in UserButtons)
        {
            if (!users.Contains(button.GetComponentInChildren<TextMeshProUGUI>().text))
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
            }
        }
        if (users.Count >= 4)
        {
            createUser.interactable = false;
        }
        else
        {
            createUser.interactable = true;
        }

    }
    void log(string text)
    {
        Debug.Log($"<size=20>{text}</size>");
    }
    public void OpenPopUp()
    {
        selectUserPopUp.SetActive(true);

    }
    public void ReturnFromPopUp() {
        selectUserPopUp.SetActive(false);
    }
    public void StartGame()
    {
        if (!(selectedUser == ""))
        {
            PlayerPrefs.SetString("User", selectedUser);
            SceneManager.LoadScene("TypeofGame", LoadSceneMode.Single);
        }
    }


}
