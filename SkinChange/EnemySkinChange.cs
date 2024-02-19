using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySkinChange : MonoBehaviour
{
    //change materials after hit
    [SerializeField] private Material[] materials;
    private Enemy2Controller health;
    private Renderer ren;
    //
    void Start()
    {
        ren = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WaitTimeTakeHitByDagger());
        IEnumerator WaitTimeTakeHitByDagger()
        {
            if ((other.gameObject.CompareTag("Sword") || other.gameObject.CompareTag("Dagger")) &&
                 PlayerControl.health > 0 && PlayerControl.isHurtingTheEnemy)
            {
                ren.sharedMaterial = materials[1];
                yield return new WaitForSeconds(0.07f);
                ren.sharedMaterial = materials[0];
            }
        }
    }
}
