using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    private GameObject[] enemies;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemy2;
    private bool allEnemiesDead; 
    [SerializeField] private GameObject wonMenu;
    public static int numOfEnemiesInLevel1 = 10;
    public static int numOfEnemiesInLevel2 = 2;
    private int numOfKilledEnemies;
    private const float z_index = -1.24f;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            enemies = new GameObject[numOfEnemiesInLevel1];
            enemies[0] = Instantiate(enemy, new Vector3(8.695017f, 1.08f, z_index), Quaternion.identity);
            enemies[1] = Instantiate(enemy, new Vector3(12.62f, 1.08f, z_index), Quaternion.identity);
            enemies[2] = Instantiate(enemy, new Vector3(26.67f, 3.313823f, z_index), Quaternion.identity);
            enemies[3] = Instantiate(enemy, new Vector3(23.14f, 1.08f, z_index), Quaternion.identity);
            enemies[4] = Instantiate(enemy, new Vector3(36.08f, 3.804378f, z_index), Quaternion.identity);
            enemies[5] = Instantiate(enemy, new Vector3(46.84f, 6.143823f, z_index), Quaternion.identity);
            enemies[6] = Instantiate(enemy, new Vector3(43.24f, 1.925041f, z_index), Quaternion.identity);
            enemies[7] = Instantiate(enemy, new Vector3(45.01f, 1.08f, z_index), Quaternion.identity);
            enemies[8] = Instantiate(enemy, new Vector3(62.69f, 1.08f, z_index), Quaternion.identity);
            enemies[9] = Instantiate(enemy, new Vector3(69.39f, 1.08f, z_index), Quaternion.identity);
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            enemies = new GameObject[numOfEnemiesInLevel2];
            enemies[0] = Instantiate(enemy2, new Vector3(6f, 1.081478f, z_index), enemy2.transform.rotation);
            enemies[1] = Instantiate(enemy, new Vector3(8f, 1.081478f, z_index), Quaternion.identity);
            foreach (GameObject enemy2 in enemies)
            {
                enemy2.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            }
        }
    }

    private void Update()
    {
        if (enemies != null)
        {
            allEnemiesDead = true;
            numOfKilledEnemies = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    allEnemiesDead = false;
                }
                if (enemies[i] == null)
                {
                    numOfKilledEnemies++;
                }
            }
            ScoreCounter.scoreValue = numOfKilledEnemies;
            if (allEnemiesDead)
            {
                wonMenu.SetActive(true);
            }
        }
    }
}

#region Just for fun
/*
enemies[25] = Instantiate(enemy, new Vector3(-2f, 1.08f, -1.24f), Quaternion.identity);
enemies[24] = Instantiate(enemy, new Vector3(-1.5f, 1.08f, -1.24f), Quaternion.identity);
enemies[23] = Instantiate(enemy, new Vector3(-1f, 1.08f, -1.24f), Quaternion.identity);
enemies[22] = Instantiate(enemy, new Vector3(-0.5f, 1.08f, -1.24f), Quaternion.identity);
enemies[21] = Instantiate(enemy, new Vector3(0f, 1.08f, -1.24f), Quaternion.identity);
enemies[20] = Instantiate(enemy, new Vector3(0.5f, 1.08f, -1.24f), Quaternion.identity);
enemies[19] = Instantiate(enemy, new Vector3(1f, 1.08f, -1.24f), Quaternion.identity);
enemies[18] = Instantiate(enemy, new Vector3(1.5f, 1.08f, -1.24f), Quaternion.identity);
enemies[17] = Instantiate(enemy, new Vector3(2f, 1.08f, -1.24f), Quaternion.identity);
enemies[16] = Instantiate(enemy, new Vector3(2.5f, 1.08f, -1.24f), Quaternion.identity);
enemies[15] = Instantiate(enemy, new Vector3(3f, 1.08f, -1.24f), Quaternion.identity);
enemies[14] = Instantiate(enemy, new Vector3(3.5f, 1.08f, -1.24f), Quaternion.identity);
enemies[13] = Instantiate(enemy, new Vector3(4f, 1.08f, -1.24f), Quaternion.identity);
enemies[12] = Instantiate(enemy, new Vector3(5f, 1.08f, -1.24f), Quaternion.identity);
enemies[11] = Instantiate(enemy, new Vector3(6f, 1.08f, -1.24f), Quaternion.identity);
enemies[10] = Instantiate(enemy, new Vector3(7f, 1.08f, -1.24f), Quaternion.identity);
*/
#endregion