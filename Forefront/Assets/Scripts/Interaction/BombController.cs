using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private bool isActive;

    [SerializeField]
    private GameObject meshObj;

    [SerializeField]
    private float explosionRadius;

    [SerializeField]
    private int explosionDamage;

    [SerializeField]
    private VisualEffect explodeVfx;

    [SerializeField]
    private Sound explodeSound;

    private void OnCollisionEnter(Collision collision)
    {
        if(isActive)
        {
            DamageNearbyEnemies();
            GameManager.visualEffectManager.StartVFX(explodeVfx);
            GameManager.audioManager.PlaySound(explodeSound);
            meshObj.SetActive(false);
            isActive = false;
            StartCoroutine(DelayInactive());
        }
    }

    private void DamageNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);

        foreach(Collider collider in colliders) //Get all nearby enemies inside the explosion radius and damage them
        {
            if(collider.CompareTag("EnemyDefault"))
            {
                EnemyEntity enemy = collider.transform.parent.GetComponent<EnemyEntity>();
                enemy.TakeDamage(explosionDamage);
            }    
        }
    }

    private IEnumerator DelayInactive() //Ensures the vfx plays and the object is reset ready for the next time it is used.
    {
        yield return new WaitForSeconds(2);
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().useGravity = false;
        meshObj.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Activate()
    {
        StartCoroutine(DelayActive());
    }

    private IEnumerator DelayActive() //To ensure the grenade does not colide with the player or hands after being thrown.
    {
        yield return new WaitForSeconds(0.2f);
        isActive = true;
    }

}
