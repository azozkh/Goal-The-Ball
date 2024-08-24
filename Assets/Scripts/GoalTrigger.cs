using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    private LevelTransitionManager transitionManager;

    void Start()
    {
        transitionManager = FindObjectOfType<LevelTransitionManager>();
        if (transitionManager == null)
        {
            Debug.LogError("LevelTransitionManager not found in the scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected with: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the goal!");  
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        if (transitionManager != null)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("Starting transition to next level: " + nextSceneIndex);
                transitionManager.ShowLevelTransition(nextSceneIndex);
            }
            else
            {
                Debug.Log("Congratulations! You've completed all levels!");
                transitionManager.ShowLevelTransition("YouWon");
            }
        }
    }

}
