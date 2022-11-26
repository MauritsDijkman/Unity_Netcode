using UnityEngine;
using System.Collections;

    public class Bullet : MonoBehaviour
    {
        [Header("Particle")]
        [SerializeField] private GameObject impactEffect;

        private void Start()
        {
            Destroy(gameObject, 5);
        }

        void OnCollisionEnter(Collision collision)
        {
        
        if (collision.gameObject.CompareTag("BulletTarget"))
                Destroy(collision.gameObject);

            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        
        GameObject hit = collision.gameObject;
        Player health = hit.GetComponent<Player>();
        if (health != null)
        {
            health.TakeDamage(10);
        }
        

        Destroy(gameObject);
        }
    }
