using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelStartPanel; 
    public float displayTime = 2.0f; 

    public void PlayGame()
    {
        StartCoroutine(DisplayLevelMessageAndLoadScene());
    }

    private IEnumerator DisplayLevelMessageAndLoadScene()
    {
        levelStartPanel.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OpenOptions()
    {
        Debug.Log("Open Options Menu");
    }
}