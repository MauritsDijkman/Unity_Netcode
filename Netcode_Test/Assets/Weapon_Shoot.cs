using UnityEngine;

public class Weapon_Shoot : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnpoint;
    [SerializeField] private Quaternion bulletRotation;
    [SerializeField] private float shootCooldown = 0.2f;
    [SerializeField] private float bulletSpeed = 10f;
    private bool canShoot;

    [Header("Effect")]
    [SerializeField] private ParticleSystem muzzleFlash;

    private void Start()
    {
        canShoot = true;
    }

    private void Update()
    {
        if (IsOwner) return;
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            canShoot = false;

            GameObject spawnedBullet = Instantiate(bullet, bulletSpawnpoint.position, bulletSpawnpoint.rotation) as GameObject;
            Rigidbody spawnedBulletRb = spawnedBullet.GetComponent<Rigidbody>();
            spawnedBulletRb.AddForce(transform.forward * bulletSpeed);
            muzzleFlash.Play();

            Invoke(nameof(ResetShoot), shootCooldown);
        }
    }

    private void ResetShoot()
    {
        canShoot = true;
    }
}
