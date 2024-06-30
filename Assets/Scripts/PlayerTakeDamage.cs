using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [Tooltip("Reference to player Health script")]
    [SerializeField] float damage_multiplier = 1.0f;
    [Tooltip("Damage multiplier for this collider")]
    [SerializeField] string bulletTag = "EnemyBullet";
    [Tooltip("Tag of objects inflicting damage")]
    [SerializeField] AudioClip hitSound;
    [Tooltip("Sound to be played when the player is hit")]
    [SerializeField] AudioSource audioSource;
    [Tooltip("Audio source for the sound")]
    [SerializeField] Animator animator;
    [Tooltip("Player animator")]
    private bool isHit;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == bulletTag)
        {
            // Animator parameter isHit to trigger animation
            animator.SetTrigger("isHit");
            // Play sound
            audioSource.PlayOneShot(hitSound);
            // Get bullet damage
            BulletStats bulletStats = collider.gameObject.GetComponent<BulletStats>();
            float bullet_damage = bulletStats.damage;
            // Calculate damage
            float damage = damage_multiplier * bullet_damage;
            // Take damage
            playerHealth.takeDamage(damage);
            // Delete bullet
            Destroy(collider.gameObject);
        }
    }
}
