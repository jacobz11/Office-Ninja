using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDestroy : MonoBehaviour
{
    [SerializeField] private GameObject orbFractured;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pot") || other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy"))
        {
            GameObject fract = Instantiate(orbFractured, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(fract, 1f);
        }
    }
}

/*            MeshCollider[] meshColliders = fract.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider collider in meshColliders)
            {
                collider.enabled = false;
            }*/

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pot") || other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Wall"))
        {
            GameObject fract = Instantiate(orbFractured, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(fract, 1f);
        }
    }*/