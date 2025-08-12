using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class IntroManager : MonoBehaviour
{
    public GameObject StartScreen;
    public float fadeDuration = 2f;
    public TextMeshProUGUI anyKeyText;
    private bool KeyPressed = false;
    private bool TitleScreenFinished = false;
    private bool fadeOut = false;
    private bool displayStartMenu = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartScreen.SetActive(true);
        StartScreen.GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(FadeIn());
        StartCoroutine(AnyKeyTextBlink());

    }
    void Update() {
        if (TitleScreenFinished)
        {
            StartCoroutine(UntilKeyPressed());

            TitleScreenFinished = false;
        }
        if (fadeOut)
        {
            StartCoroutine(FadeOut());
            fadeOut = false;
        }
        if (displayStartMenu) {
            SceneManager.LoadScene("SampleScene");

            displayStartMenu = false;
        }
    }

    IEnumerator FadeIn()
    {
        float timePassed = 0f;
        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            StartScreen.GetComponent<CanvasGroup>().alpha = timePassed / fadeDuration;
            yield return null;
        }
        TitleScreenFinished = true;

    }
    IEnumerator AnyKeyTextBlink()
    {
        while (KeyPressed == false)
        {
            if (anyKeyText.gameObject.activeInHierarchy)
            {
                anyKeyText.gameObject.SetActive(false);
            }
            else
            {
                anyKeyText.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
    IEnumerator UntilKeyPressed()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        fadeOut = true;
        KeyPressed = true;

    }
    IEnumerator FadeOut()
    {
        float timePassed = 0f;
        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            StartScreen.GetComponent<CanvasGroup>().alpha = 1 - (timePassed / fadeDuration);
            yield return null;
        }
        displayStartMenu = true;


    }
}
