using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class GunManagerOVR : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public float destroyAfterSeconds = 5;
    public Transform bulletSpawnPoint;

    public OVRInput.Button shootButton;

    private OVRGrabbable ovrGrabbable;
    
    // Start is called before the first frame update
    void Start()
    {
        OVRGrabbable ovrGrabbable = GetComponent<OVRGrabbable>();
        if (ovrGrabbable == null)
        {
            Debug.LogError("Cannot get OVRGrabbable component component.");
        }
        // // FireBullet is the function that gets called
        // grabbable.activated.AddListener(FireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        if (ovrGrabbable != null)
        {
            // if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            // if(ovrGrabbable.isGrabbed && OVRInput.GetDown(shootButton, ovrGrabbable.grabbedBy.GetController()))
            // if(ovrGrabbable.isGrabbed && OVRInput.GetDown(shootButton))
            // if(OVRInput.GetDown(shootButton))
            if(ovrGrabbable.isGrabbed)
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
