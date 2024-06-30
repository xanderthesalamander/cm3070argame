using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] float damage_multiplier = 1.0f;
    [SerializeField] string bulletTag = "Bullet";
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;
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
