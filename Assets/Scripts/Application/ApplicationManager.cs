using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager: MonoBehaviour
{
    #region Singleton
    public static ApplicationManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public GameObject pauseMenu;
    public bool isApplicationPause = false;

    public void TogglePauseGame()
    {
        if (isApplicationPause)
        {
            Time.timeScale = 1;
            isApplicationPause = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isApplicationPause = true;
            pauseMenu.SetActive(true);
        }
    }

    public void GameFinish()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 0;
        isApplicationPause = true;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
