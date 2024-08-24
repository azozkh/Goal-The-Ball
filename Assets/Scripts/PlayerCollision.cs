using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    public int playerHealth = 100; 
    public TextMeshProUGUI healthText; 
    public int damageAmount = 10; 

    private LevelTransitionManager transitionManager;

    void Start()
    {
        UpdateHealthText();

        transitionManager = FindObjectOfType<LevelTransitionManager>();
        if (transitionManager == null)
        {
            Debug.LogError("LevelTransitionManager not found in the scene!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DecreaseHealth();
        }
    }

    public void DecreaseHealth()
    {
        playerHealth -= damageAmount;

        UpdateHealthText();

        if (playerHealth <= 0)
        {
            Debug.Log("Player has died!");
            RestartGame();
        }
    }

    public void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + playerHealth.ToString();
        }
        else
        {
            Debug.LogError("Health Text UI element not assigned.");
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game from level 1...");
        if (transitionManager != null)
        {
            transitionManager.ShowLevelTransition(1); 
        }
        else
        {
            SceneManager.LoadScene(1); 
        }
    }

    public void RestoreHealth(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 100); 
        UpdateHealthText(); 

        Debug.Log("Health restored by " + amount + ". Current health: " + playerHealth);
    }
}
