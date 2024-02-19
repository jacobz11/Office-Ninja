using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            offset = new Vector3(0f, 3.9f, -9.2f);
            transform.position = target.position + offset;
        }
        transform.position = target.position + offset;
    }
}
