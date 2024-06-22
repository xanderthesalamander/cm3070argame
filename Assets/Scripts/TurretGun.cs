using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGun : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    private BulletStats bulletStats;
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        bulletStats = bullet.GetComponent<BulletStats>();
    }

    public void FireBullet() {
        // Play sound
        sound.Play();
        // Create bullet
        GameObject spawnedBullet = Instantiate(bullet);
        // Place it in bulletSpawnPoint
        spawnedBullet.transform.position = bulletSpawnPoint.position;
        // Give it initial velocity
        spawnedBullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletStats.speed;
        // Destroy the bullet after 5 seconds
        Destroy(spawnedBullet, 5);
    }
}
