using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip clip;
    public void PlayGame()
    {
        src.clip = clip;
        src.Play();
        StartCoroutine(waitTimeForMenuSound());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;

    }
    IEnumerator waitTimeForMenuSound()
    {
        yield return new WaitForSeconds(0.3f);
    }
    public void QuitGame()
    {
        src.clip = clip;
        src.Play();
        Application.Quit();
    }
}
