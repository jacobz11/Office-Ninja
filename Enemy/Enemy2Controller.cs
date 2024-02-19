using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Enemy2Controller : MonoBehaviour
{
    private Transform player;
    private float distance;
    private float speed = 4.5f;
    private Animator animator;
    private Vector3 movementDirection;
    private Rigidbody rbEnemy;
    [SerializeField] private bool isBumpedIntoWall;
    //health update
    [SerializeField] private Slider healthBarSlider;
    private float health = 20f;
    private float maxHealth = 20f;
    private float damageAmount = 2f;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject Fill;
    [SerializeField] private BoxCollider bxColEnemySkin;
    private Transform healthRotation;
    //health end
    //sounds
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip enemyHurt, deathSound, footSteps;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        rbEnemy = GetComponent<Rigidbody>();
        healthRotation = GameObject.Find("HealthRotation").transform;
    }
    public void PlayFootStepsSound()
    {
        src.clip = footSteps;
        src.Play();
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        if (health > 0)
        {
            healthBar.transform.rotation = healthRotation.rotation;
            movementDirection = transform.forward;
            if (distance < 12)
            {
                animator.SetBool("isRun", true);
                transform.Translate(speed * Time.deltaTime * movementDirection, Space.World);
            }
        }
        else
            animator.SetBool("isDead", true);
        if (6f < distance && distance < 9f || isBumpedIntoWall)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, transform.position.z));
        }
        isBumpedIntoWall = false;
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBarSlider.value = currentValue / maxValue;
    }
    public void DestroyHealthBar()
    {
        healthBar.SetActive(false);
        rbEnemy.useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        src.clip = deathSound;
        src.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dagger") && PlayerControl.health > 0
             && PlayerControl.isHurtingTheEnemy)
        {
            StartCoroutine(waitForBloodSplash());
            IEnumerator waitForBloodSplash()
            {
                blood.SetActive(true);
                src.clip = enemyHurt;
                src.Play();
                yield return new WaitForSeconds(0.61f);
                blood.SetActive(false);
            }
            health -= damageAmount;
            UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                Fill.SetActive(false);
                blood.SetActive(false);
                bxColEnemySkin.enabled = false;
                Destroy(gameObject, 2.2f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            isBumpedIntoWall = true;
    }
}
