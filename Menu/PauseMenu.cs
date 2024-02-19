using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu, wonMenu, restartMenu, tutorials;
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip clip;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                tutorials.SetActive(true);
                Resume();
            }
            else
            {
                tutorials.SetActive(false);
                Pause();
            }
        }
        if (PlayerControl.health <= 0)
        {
            restartMenu.SetActive(true);
            tutorials.SetActive(false);
        }
    }

    public void Resume()
    {
        src.clip = clip;
        src.Play();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        tutorials.SetActive(true);
    }
    public void Restart()
    {
        src.clip = clip;
        src.Play();
        ScoreCounter.scoreValue = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        restartMenu.SetActive(false);
    }
    public void NextLevel()
    {
        src.clip = clip;
        src.Play();
        ScoreCounter.scoreValue = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        wonMenu.SetActive(false);
    }
    public void Pause()
    {
        src.clip = clip;
        src.Play();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void PlayGame()
    {
        ScoreCounter.scoreValue = 0;
        StartCoroutine(WaitSomeTimeForMenuSound());
        SceneManager.LoadScene(0);
    }
    IEnumerator WaitSomeTimeForMenuSound() 
    {
        src.clip = clip;
        src.Play();
        yield return new WaitForSeconds(5f);
    }
    public void QuitGame()
    {
        src.clip = clip;
        src.Play();
        Application.Quit();
    }
}
