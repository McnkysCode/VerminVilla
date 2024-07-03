using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private AudioSource buttonAudio;
    private Animator animButton;
    private bool isClicked = false;
   
    
    public void StartGame()
    {
        if (!isClicked)
        {
            isClicked = true;
            DelayStart();
        }
    }
    public void Credits()
    {
        if (!isClicked)
        {
            isClicked = true;
            DelayCredits();
        }
    }
    public void QuitGame()
    {
        if (!isClicked)
        {
            isClicked = true;
            Application.Quit();
        }
    }
    public void BackToMenu()
    {
        if (!isClicked)
        {
            isClicked = true;
            DelayBackToMenu();
        }
    }
    public void EndMenu()
    {
        if (!isClicked)
        {
            isClicked = true;
            DelayEnd();
        }
    }
    void DelayStart()
    {
        if (animButton != null)
            animButton.SetBool("Pressed", true);
        buttonAudio.Play();
        SceneManager.LoadScene("Main");
        isClicked = false;
    }

    void DelayCredits()
    {
        if (animButton != null)
            animButton.SetBool("Pressed", true);
        buttonAudio.Play();
        SceneManager.LoadScene("Credits");
        isClicked = false;
    }
    void DelayBackToMenu()
    {
        if (animButton != null)
            animButton.SetBool("Pressed", true);
        buttonAudio.Play();
        SceneManager.LoadScene("MainMenu");
        isClicked = false;
    }

    void DelayEnd()
    {
        if (animButton != null)
            animButton.SetBool("Pressed", true);
        buttonAudio.Play();
        SceneManager.LoadScene("EndMenu");
        isClicked = false;
    }
}
