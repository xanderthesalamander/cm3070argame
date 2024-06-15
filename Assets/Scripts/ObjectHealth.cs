using UnityEngine;
using UnityEngine.UI;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] int resourceValue = 10;
    [SerializeField] AudioClip deadSound;
    [SerializeField] Slider slider;
    private float health = 100f;
    private ResourceManager resourceManager;
    private GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
        resourceManager = FindObjectOfType<ResourceManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        slider.value = CalculateHealth();
        // Destroy when no health
        if (health <= 0)
        {
            // Play dead sound at current position
            PlayDeadSound();
            // Add resource
            resourceManager.AddResource(resourceValue);
            // Destroy the GameObject
            Destroy(gameObject);
        }
        // For healing
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    void PlayDeadSound()
    {
        // Create new GameObject to handle the sound
        // This is in order to get rid of the object while still playing the sound
        GameObject audioObject = new GameObject("DeadSoundObject");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(deadSound);
        // Destroy audio source when sound finished
        Destroy(audioObject, deadSound.length);
    }

    // Returns health as a float between 0 and 1
    float CalculateHealth()
    {
        return health / maxHealth;
    }

    // Take damage
    public void takeDamage(float damage)
    {
        health -= damage;
    }

    // Heal
    public void heal(float healthValue)
    {
        health += healthValue;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
