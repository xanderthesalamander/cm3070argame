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

    // void OnCollisionEnter(Collision collision)
    void OnTriggerEnter(Collider collider)
    {
        // if (collision.gameObject.tag == bulletTag)
        if (collider.gameObject.tag == bulletTag)
        {
            // Play sound
            audioSource.PlayOneShot(hitSound);
            // Get bullet damage
            // BulletStats bulletStats = collision.gameObject.GetComponent<BulletStats>();
            BulletStats bulletStats = collider.gameObject.GetComponent<BulletStats>();
            float bullet_damage = bulletStats.damage;
            // Calculate damage
            float damage = damage_multiplier * bullet_damage;
            // Take damage
            objectHealth.takeDamage(damage);
            // Delete bullet
            Destroy(collider.gameObject);
            // Destroy(collision.gameObject);
        }
    }
}
