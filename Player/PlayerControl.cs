using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //private bool shoot;
    private Animator anim;
    private Rigidbody playerRb;
    private float speed = 7f;
    private float jumpSpeed = 32f;
    private float yGravitySpeed;
    [SerializeField] bool isGrounded;
    private BoxCollider bxcol;
    private bool isCrouching;

    //Ground
    private float groundDistance = 0.3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    //End ground

    //For player's Health
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject sliderCanvas;
    private float maxHealth = 10f;
    public static float health;
    private float damageAmount = 2f;
    [SerializeField] private GameObject Fill;
    //end Health

    //audio
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip orbHit, heal, steps, land, swordSlash;
    private bool isStep;
    //end audio
    public static bool isHurtingTheEnemy;

    [SerializeField] private GameObject trailsEffect;

    [SerializeField] private GameObject howToMoveAD, jumpTutorial, attackTutorial, crouchTutorial;

    [SerializeField] private GameObject DaggerPrefab;
    [SerializeField] private Transform DaggerSpawnPoint;
    [SerializeField] private Transform DaggerDirection;
    private void Start()
    {
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        bxcol = GetComponent<BoxCollider>();
        health = maxHealth;
        UpdateHealthBar(health, maxHealth);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            howToMoveAD.SetActive(false);
        }
        //Cursor.lockState = CursorLockMode.Locked;
        isStep = false;
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movementDirection = new(horizontalInput, 0, 0);
        movementDirection.Normalize();
        anim.SetFloat("Move", horizontalInput);
        if (health > 0 && sliderCanvas != null)
        {
            if (horizontalInput != 0f && !isCrouching)
            {
                StartCoroutine(WaitForPlayFootStepSound());
                transform.Translate(speed * Time.deltaTime * movementDirection, Space.World);
                transform.forward = movementDirection;
            }
            else
            {
                anim.SetFloat("Move", 0);
            }

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                StartCoroutine(WaitForSwordSlashToDisappear());
                IEnumerator WaitForSwordSlashToDisappear()
                {
                    if (Input.GetKey(KeyCode.F) && isGrounded && horizontalInput <= 0.7f
                        && horizontalInput >= -0.7f)
                    {
                        isHurtingTheEnemy = true;
                        anim.SetBool("isAttack", true);
                        trailsEffect.SetActive(true);
                    }
                    if (Input.GetKeyUp(KeyCode.F))
                    {
                        isHurtingTheEnemy = false;
                        anim.SetBool("isAttack", false);
                        yield return new WaitForSeconds(0.33f);
                        trailsEffect.SetActive(false);
                    }
                }
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                isHurtingTheEnemy = true;
                if (Input.GetKey(KeyCode.F) && isGrounded && horizontalInput == 0f)
                {
                    //attackTutorial.SetActive(false);
                    anim.SetBool("isThrow", true);
                }
                if (Input.GetKeyUp(KeyCode.F))
                {
                    anim.SetBool("isThrow", false);
                }
            }
                yGravitySpeed += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isDead", true);
        }


        #region Jumping animation and logic
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && !isCrouching)
        {
            yGravitySpeed = 0f;
            anim.SetBool("isLand", true);
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && health > 0)
            {
                yGravitySpeed = jumpSpeed;
                anim.SetBool("isJump", true);
            }


            bxcol.center = new Vector3(bxcol.center.x, 0.8734166f, bxcol.center.z);
            bxcol.size = new Vector3(bxcol.size.x, 1.749791f, bxcol.size.z);
            playerRb.AddForce(Vector3.up * yGravitySpeed, ForceMode.Impulse);
        }
        else
        {
            bxcol.center = new Vector3(bxcol.center.x, 0.9164609f, bxcol.center.z);
            bxcol.size = new Vector3(bxcol.size.x, 0.6756009f, bxcol.size.z);
            anim.SetBool("isLand", false);
            if (yGravitySpeed < 2f)
            {
                anim.SetBool("isFall", true);
            }
        }
        #endregion

        #region crouching
        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.F))
        {
            anim.SetBool("isCrouch", true);
            bxcol.center = new Vector3(bxcol.center.x, 0.4488836f, bxcol.center.z);
            bxcol.size = new Vector3(bxcol.size.x, 0.900725f, bxcol.size.z);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("isCrouch", false);
            bxcol.center = new Vector3(bxcol.center.x, 0.8734166f, bxcol.center.z);
            bxcol.size = new Vector3(bxcol.size.x, 1.749791f, bxcol.size.z);
            isCrouching = false;
        }
        #endregion
    }
    #region shoot dagger
    public void ShootDagger()
    {
        GameObject dagger = Instantiate(DaggerPrefab, DaggerSpawnPoint.position, DaggerPrefab.transform.rotation);
        if (transform.rotation.y < 0)
        {
            dagger.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        dagger.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        dagger.GetComponent<Rigidbody>().useGravity = false;
        dagger.GetComponent<Transform>().parent = DaggerSpawnPoint;
        dagger.GetComponent<BoxCollider>().enabled = false;
        dagger.GetComponent<BoxCollider>().enabled = true;
        dagger.GetComponent<Transform>().parent = null;
        dagger.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Destroy(dagger, 2f);
        Vector3 forceDirection = DaggerDirection.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(DaggerDirection.position, DaggerDirection.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
                forceDirection = (hit.point - DaggerSpawnPoint.position).normalized;
        }
        dagger.GetComponent<Rigidbody>().AddForce(forceDirection * 15f, ForceMode.Impulse);
    }
    #endregion
    IEnumerator WaitForPlayFootStepSound()
    {
        yield return new WaitUntil(() => isStep == true);
    }
    public bool PlayFootStepSound()
    {
        isStep = true;
        src.clip = steps;
        src.Play();
        return isStep;
    }
    public void PlayLandSound()
    {
        src.clip = land;
        src.Play();
    }
    public void PlaySwordSlashSound()
    {
        src.clip = swordSlash;
        src.Play();
    }
    #region animation event crouching
    public bool IsCrouchingAnim()
    {
        isCrouching = true;
        return isCrouching;
    }
    #endregion

    #region Health Script
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Orb") || collider.gameObject.CompareTag("Enemy2"))
        {
            PlayerSkinChange.orbInsidePlayer = true;
            src.clip = orbHit;
            src.Play();
            health -= damageAmount;
            UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                Fill.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Heal"))
        {
            src.clip = heal;
            src.Play();
            health = maxHealth;
            UpdateHealthBar(health, maxHealth);
        }
        if (other.gameObject.CompareTag("HowToJump"))
        {
            jumpTutorial.SetActive(true);
        }
        if (other.gameObject.CompareTag("HowToAttack"))
        {
            attackTutorial.SetActive(true);
        }
        if (other.gameObject.CompareTag("HowToCrouch"))
        {
            crouchTutorial.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("HowToJump"))
        {
            jumpTutorial.SetActive(false);
        }
        if (other.gameObject.CompareTag("HowToAttack"))
        {
            attackTutorial.SetActive(false);
        }
        if (other.gameObject.CompareTag("HowToCrouch"))
        {
            crouchTutorial.SetActive(false);
        }
    }
    #endregion

    #region Dead Animation Events
    public void StartDeadAnimation()
    {
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        bxcol.enabled = false;
        playerRb.isKinematic = true;
    }
    public void DestroySlider()
    {
        sliderCanvas.SetActive(false);
    }
    public void EndDeadAnimation()
    {
        anim.SetBool("isDead", false);
        ScoreCounter.scoreValue = 0;
    }
    #endregion

    /*    public GameObject swordPrefab;
    private Transform spawnWeapon;
    private Vector3 swordRotation = new Vector3(122f, 0f, 90f);
    private Quaternion swordQuaternion;*/

    /*      spawnWeapon = GameObject.Find("SpawnWeapon").transform;
        swordQuaternion = Quaternion.Euler(swordRotation);*/

    /*            if (Input.GetKey(KeyCode.Alpha1) && spawnWeapon.childCount < 1)
            {
                //GameObject sword = Instantiate(swordPrefab, spawnWeapon.transform.position, swordPrefab.transform.rotation);
                //sword.transform.Rotate(swordRotation);
                //sword.GetComponent<Transform>().parent = spawnWeapon;
            } */

    //public Camera cam;
    //public Transform healthBar;

    //Uplade()
    //healthBar.rotation = cam.transform.rotation;

    /*    //weapons
        public GameObject swordPrefab;
        public Transform weaponSpawnPoint;
        public Quaternion swordRotation;
        //private Transform swordRotation = new Vector3(15.234f, -496.619f, -92.038f);
        //end weapons*/
}
/*       if (Input.GetKey(KeyCode.Alpha1) && weaponSpawnPoint.childCount < 1)
        {
            //swordRotation = Quaternion.Euler(-13.461f, -537.42f, -269.721f);
            GameObject sword = Instantiate(swordPrefab, weaponSpawnPoint.position, Quaternion.Euler(-2.464f, -715.137f, -442.273f));
            sword.GetComponent<Transform>().parent = weaponSpawnPoint;
        }*/