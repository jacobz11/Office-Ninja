using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableDestroy : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject fractured;
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Sword") && Input.GetKey(KeyCode.F)) || collision.gameObject.CompareTag("Orb")
            || collision.gameObject.CompareTag("Enemy2"))
        {
            source.clip = clip;
            source.Play();
            GameObject fract = Instantiate(fractured, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(fract, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Sword") && Input.GetKey(KeyCode.F)) ||
            other.gameObject.CompareTag("Orb") || other.gameObject.CompareTag("Dagger"))
        {
            source.clip = clip;
            source.Play();
            GameObject fract = Instantiate(fractured, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(fract, 1f);
        }
    }
}