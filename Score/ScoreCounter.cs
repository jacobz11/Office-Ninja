using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour
{
    public static int scoreValue = 0;
    private Text scoreText;
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            scoreText.text = "Kills: " + scoreValue + "/" + EnemySpawner.numOfEnemiesInLevel1;
        if (SceneManager.GetActiveScene().buildIndex == 2)
            scoreText.text = "Kills: " + scoreValue + "/" + EnemySpawner.numOfEnemiesInLevel2;
    }
}
