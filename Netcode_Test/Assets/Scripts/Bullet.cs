using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Particle")]
    [SerializeField] private GameObject impactEffect;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BulletTarget"))
            Destroy(collision.gameObject);

        Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));

        Destroy(gameObject);
    }
}
