using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class GunManager : MonoBehaviour
{
    public OVRInput.Button shootButton;
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public float destroyAfterSeconds = 5;
    public Transform bulletSpawnPoint;

    private bool canShootLeft = false;
    private bool canShootRight = false;

    void OnTriggerEnter(Collider collider)
    {
        // Ugly way to check if in hand and pressing the trigger
        // Enables shooting if controllers grab triggers are colliding
        if (collider.gameObject.name == "ControllerGrabLocation")
        {
            if (collider.gameObject.transform.parent.parent.parent.name == "LeftController")
            {
                canShootLeft = true;
            }
            if (collider.gameObject.transform.parent.parent.parent.name == "RightController")
            {
                canShootRight = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        Debug.LogError("Exit: " + collider.gameObject.name);
        if (collider.gameObject.name == "ControllerGrabLocation")
        {
            if (collider.gameObject.transform.parent.parent.parent.name == "LeftController")
            {
                Debug.LogError("Can not shoot left");
                canShootLeft = false;
            }
            if (collider.gameObject.transform.parent.parent.parent.name == "RightController")
            {
                Debug.LogError("Can not shoot right");
                canShootRight = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canShootLeft)
        {
            if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                FireBullet();
            }
        }
        if (canShootRight)
        {
            if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                FireBullet();
            }
        }
    }

    // Fire bullet
    void FireBullet()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody spawnedBulletRB = spawnedBullet.GetComponent<Rigidbody>();
        // Place it in bulletSpawnPoint and give it initial velocity
        spawnedBullet.transform.position = bulletSpawnPoint.position;
        spawnedBulletRB.velocity = bulletSpawnPoint.transform.forward * bulletSpeed;
        // Destroy the bullet
        Destroy(spawnedBullet, destroyAfterSeconds);
    }
}
