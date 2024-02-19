using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pot") || other.gameObject.CompareTag("Wall") || 
            other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ground") ||
            other.gameObject.CompareTag("Enemy2"))
            Destroy(gameObject);
    }
}
