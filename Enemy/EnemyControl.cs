using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    private bool shoot;
    private Animator anim;
    private float distance;
    private Transform player;
    [SerializeField] private GameObject OrbPrefab;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Transform ThrowDirection;

    //Health properties
    [SerializeField] private Slider slider;
    private float health = 20f;
    private float maxHealth = 20f;
    private float damageAmount = 2f;
    [SerializeField] private GameObject Fill;
    [SerializeField] private GameObject healthCanvas;
    private Transform healthRotation;
    [SerializeField] private Transform healthBar;
    //end health
    [SerializeField] private float orbSpeed;
    //audio
    //[SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip shootOrb, deathSound, zHurt;
    [SerializeField] private BoxCollider bxColEnemySkin;
    //end audio
    [SerializeField] private GameObject blood;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        healthRotation = GameObject.Find("HealthRotation").transform;
    }

    void Update()
    {
        if (health > 0f)
        {
            healthBar.rotation = healthRotation.rotation;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, transform.position.z));
            distance = Vector3.Distance(transform.position, player.position);
        }
        if (distance < 7f)
            anim.SetBool("isShoot", true);
        else anim.SetBool("isShoot", false);

        if (health <= 0)
        {
            anim.SetBool("isDead", true);
            Destroy(gameObject, 2f);
        }
    }
    public void ShootOrb()
    {
        StartCoroutine(waitForAnimation());
        IEnumerator waitForAnimation()
        {
            shoot = false;
            GameObject orb = Instantiate(OrbPrefab, SpawnPoint.position, Quaternion.identity);
            orb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            orb.GetComponent<Rigidbody>().useGravity = false;
            orb.GetComponent<Transform>().parent = SpawnPoint;
            orb.GetComponent<SphereCollider>().enabled = false;
            yield return new WaitUntil(() => shoot == true);
            orb.GetComponent<SphereCollider>().enabled = true;
            orb.GetComponent<Transform>().parent = null;
            orb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(orb, 3f);
            Vector3 forceDirection = ThrowDirection.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(ThrowDirection.position, ThrowDirection.forward, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                    forceDirection = (hit.point - SpawnPoint.position).normalized;
            }
            orb.GetComponent<Rigidbody>().AddForce(forceDirection * orbSpeed, ForceMode.Impulse);
        }
    }
    public bool ReadyToShootOrb()
    {
        shoot = true;
        return shoot;
    }
    public void PlayShootSound()
    {
        src.clip = shootOrb;
        src.Play();
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Sword") || other.gameObject.CompareTag("Dagger")) && 
             PlayerControl.health > 0 && PlayerControl.isHurtingTheEnemy) //Input.GetKey(KeyCode.F) &&
        {
            StartCoroutine(waitForBloodSplash());
            IEnumerator waitForBloodSplash()
            {
                blood.SetActive(true);
                src.clip = zHurt;
                src.Play();
                yield return new WaitForSeconds(0.61f);
                blood.SetActive(false);
            }
            health -= damageAmount;
            UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                Fill.SetActive(false);
                bxColEnemySkin.enabled = false;
            }
        }
    }

    #region Dead Animation Events
    public void StartDeadAnimation()
    {
        src.clip = deathSound;
        src.Play();
        GetComponent<BoxCollider>().enabled = false;
    }
    public void DestroySlider()
    {
        healthCanvas.SetActive(false);
    }
    public void EndDeadAnimation()
    {
        Destroy(gameObject, 0.2f);
    }
    #endregion
}
