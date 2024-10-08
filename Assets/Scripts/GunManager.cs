using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using TMPro;

public class GunManager : MonoBehaviour
{
    [SerializeField] public GameObject bullet;
    [SerializeField] private int bulletsPerTrigger = 1;
    [SerializeField] public Transform bulletSpawnPoint;
    [SerializeField] public AudioClip gunShotAudio;
    [SerializeField] private TextMeshProUGUI debugScreenText;
    private bool canShootLeft = false;
    private bool canShootRight = false;
    private BulletStats bulletStats;
    private int bulletShotCount = 0;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bulletStats = bullet.GetComponent<BulletStats>();
    }

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
        if (collider.gameObject.name == "ControllerGrabLocation")
        {
            if (collider.gameObject.transform.parent.parent.parent.name == "LeftController")
            {
                canShootLeft = false;
            }
            if (collider.gameObject.transform.parent.parent.parent.name == "RightController")
            {
                canShootRight = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debugging
        DebugGun();
        // Shooting
        tryShoot();
    }

    // Shooting
    private void tryShoot()
    {
        if (canShootLeft)
        {
            if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                bulletShotCount = bulletsPerTrigger;
                InvokeRepeating("FireBullet", 0.0f, 0.04f);
            }
        }
        if (canShootRight)
        {
            if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                bulletShotCount = bulletsPerTrigger;
                InvokeRepeating("FireBullet", 0.0f, 0.04f);
            }
        }
    }

    // Fire bullet
    void FireBullet()
    {
        if (audioSource != null)
        {
            // Play sound
            audioSource.PlayOneShot(gunShotAudio);
        }
        GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody spawnedBulletRB = spawnedBullet.GetComponent<Rigidbody>();
        // Place it in bulletSpawnPoint and give it initial velocity
        spawnedBullet.transform.position = bulletSpawnPoint.position;
        spawnedBulletRB.velocity = bulletSpawnPoint.transform.forward * bulletStats.speed;
        // Destroy the bullet
        Destroy(spawnedBullet, bulletStats.destroyAfterSeconds);
        // Cancel shooting if finished the bullets per trigger
        if (--bulletShotCount == 0)
        {
            CancelInvoke("FireBullet");
        }
    }

    public void DebugGun()
    {
        if (debugScreenText != null)
        {
            string debuggingText = "Debug:";
            debuggingText += "\n";
            debuggingText += "\ncanShootLeft = " + canShootLeft.ToString();
            debuggingText += "\ncanShootRight = " + canShootRight.ToString();
            debuggingText += "\n";
            debuggingText += "\nBullet Stats:";
            debuggingText += "\nBullet Speed = " + bulletStats.speed.ToString();
            debuggingText += "\nBullet Duration = " + bulletStats.destroyAfterSeconds.ToString() + "s";
            debugScreenText.text = debuggingText;
        }
    }
}
