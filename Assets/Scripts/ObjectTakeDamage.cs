using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTakeDamage : MonoBehaviour
{
    [SerializeField] ObjectHealth objectHealth;
    [SerializeField] float damage_multiplier = 1.0f;
    [SerializeField] string bulletTag = "Bullet";
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioSource audioSource;

    // void Start()
    // {
    //     audioSource = GetComponent<AudioSource>();
    // }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == bulletTag)
        {
            // Play sound
            Debug.Log("1");
            audioSource.PlayOneShot(hitSound);
            // Get bullet damage
            Debug.Log("2");
            BulletStats bulletStats = collision.gameObject.GetComponent<BulletStats>();
            float bullet_damage = bulletStats.damage;
            // Calculate damage
            float damage = damage_multiplier * bullet_damage;
            // Take damage
            Debug.Log("3");
            objectHealth.takeDamage(damage);
            // Delete bullet
            Debug.Log("4");
            Destroy(collision.gameObject);
        }
    }
}