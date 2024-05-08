using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using TMPro;

public class GunManager : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawnPoint;

    [SerializeField] private TextMeshProUGUI debugScreenText;
    private bool canShootLeft = false;
    private bool canShootRight = false;
    private BulletStats bulletStats;

    void Start()
    {
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
        debug();
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
        GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody spawnedBulletRB = spawnedBullet.GetComponent<Rigidbody>();
        // Place it in bulletSpawnPoint and give it initial velocity
        spawnedBullet.transform.position = bulletSpawnPoint.position;
        spawnedBulletRB.velocity = bulletSpawnPoint.transform.forward * bulletStats.speed;
        // Destroy the bullet
        Destroy(spawnedBullet, bulletStats.destroyAfterSeconds);
    }

    public void debug()
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
