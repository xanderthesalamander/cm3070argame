using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] Slider slider;
    [SerializeField] AudioClip deadSound;
    private float health = 100f;

    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Awake()
    {
        // Subscribe to the game manager
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.GameStart)
        {
            // Start game
            health = maxHealth;
        }
    }

    void Update()
    {
        slider.value = CalculateHealth();
        // Player dead
        if (health <= 0)
        {
            // Play dead sound at current position
            PlayDeadSound();
            // Game Over
            GameManager.instance.UpdateGameState(GameState.LoseState);
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
