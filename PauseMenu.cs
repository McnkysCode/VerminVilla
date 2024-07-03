using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private AudioSource pauseMenuAudio;
    private bool isPaused;

    private void Awake()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Debug.Log("unPaused");
                ResumeGame();
            }
            else
            {
                PauseGame();
                Debug.Log("paused");
            }
        }
    }

    private void PauseGame()
    {
        panel.SetActive(true);
        pauseMenuAudio.Play();
        Time.timeScale = 0f;
        isPaused = true;
        //FindAnyObjectByType<Movement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ResumeGame()
    {
        panel.SetActive(false);
        pauseMenuAudio.Play();
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGameButton()
    {
        if (isPaused)
        {
            ResumeGame();
            pauseMenuAudio.Play();
        }
    }

    public void MainMenuButton()
    {
        if (isPaused)
        {
            SceneManager.LoadScene("MainMenu");
            pauseMenuAudio.Play();
        }
    }

    public void QuitButton()
    {
        if (isPaused)
        {
            Application.Quit();
            pauseMenuAudio.Play();
        }
    }
}