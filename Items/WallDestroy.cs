using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroy : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject fracturedWall;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sword") && PlayerControl.isHurtingTheEnemy
            || collision.gameObject.CompareTag("Enemy2"))
        {
            source.clip = clip;
            source.Play();
            GameObject fract = Instantiate(fracturedWall, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(fract, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Sword") || other.gameObject.CompareTag("Dagger"))
            && PlayerControl.isHurtingTheEnemy)
        {
            source.clip = clip;
            source.Play();
            GameObject fract = Instantiate(fracturedWall, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(fract, 1f);
        }
    }
}

            //GameObject fract = Instantiate(fracturedWall, new Vector3(transform.position.x, transform.position.y - 2.8f, transform.position.z), transform.rotation);