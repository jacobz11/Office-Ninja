using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkinChange : MonoBehaviour
{
    //change materials after hit
    [SerializeField] Material[] material;
    public static bool orbInsidePlayer;
    private Renderer rend;
    //
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (PlayerControl.health >= 0)
        {
            StartCoroutine(WaitTimeTakeHit());
        }
        IEnumerator WaitTimeTakeHit()
        {
            if (orbInsidePlayer)
            {
                rend.sharedMaterial = material[1];
                yield return new WaitForSeconds(0.07f);
                rend.sharedMaterial = material[0];
                orbInsidePlayer = false;
            }
        }
    }
}

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerControl.health -= 2f;
        }
    }*/
