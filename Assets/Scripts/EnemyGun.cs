using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject bullet;
    public AudioClip gunShotAudio;
    public Transform bulletSpawnPoint;
    private BulletStats bulletStats;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bulletStats = bullet.GetComponent<BulletStats>();
    }

    public void FireBullet() {
        // Play sound
        audioSource.PlayOneShot(gunShotAudio);
        // Create bullet
        GameObject spawnedBullet = Instantiate(bullet);
        // Place it in bulletSpawnPoint
        spawnedBullet.transform.position = bulletSpawnPoint.position;
        // Give it initial velocity
        spawnedBullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletStats.speed;
        // Destroy the bullet after 3 seconds
        Destroy(spawnedBullet, 3);
    }
}
