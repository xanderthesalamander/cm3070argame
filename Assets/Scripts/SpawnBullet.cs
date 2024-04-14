using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public float destroyAfterSeconds = 5;

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody spawnedBulletRB = spawnedBullet.GetComponent<Rigidbody>();
            spawnedBulletRB.velocity = transform.forward * bulletSpeed;
            // Destroy the bullet
            Destroy(spawnedBullet, destroyAfterSeconds);
        }
    }
}
