using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseButtonUI;
    public AudioMixer audioMixer; 
    private float originalVolume = 0f; 

    void Start()
    {
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);
        audioMixer.GetFloat("MyExposedParam", out originalVolume);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        pauseButtonUI.SetActive(false); 
        audioMixer.SetFloat("MyExposedParam", -80f);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true); 
        audioMixer.SetFloat("MyExposedParam", originalVolume);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        audioMixer.SetFloat("MyExposedParam", originalVolume);
        SceneManager.LoadScene("MainMenu");
    }
}
