using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.Audio;

public class LevelTransitionManager : MonoBehaviour
{
    public GameObject transitionPanel; 
    public TextMeshProUGUI levelText;  
    public float transitionDuration = 2.0f; 
    public AudioMixer audioMixer;  

    private CanvasGroup canvasGroup;
    private float previousVolume;  
    void Start()
    {
        canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
        transitionPanel.SetActive(false); 
    }

    public void ShowLevelTransition(int nextSceneIndex)
    {
        StartCoroutine(LevelTransitionCoroutine(nextSceneIndex));
    }

    public void ShowLevelTransition(string nextSceneName)
    {
        StartCoroutine(LevelTransitionCoroutine(nextSceneName));
    }

    private IEnumerator LevelTransitionCoroutine(int sceneIndex)
    {
        levelText.text = "Level " + sceneIndex.ToString(); 

        audioMixer.GetFloat("MyExposedParam", out previousVolume);

        audioMixer.SetFloat("MyExposedParam", -80f);  

        transitionPanel.SetActive(true);
        canvasGroup.alpha = 0f; 

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / transitionDuration;
            yield return null;
        }

        yield return new WaitForSeconds(transitionDuration);

        yield return SceneManager.LoadSceneAsync(sceneIndex);

        audioMixer.SetFloat("MyExposedParam", previousVolume);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / transitionDuration;
            yield return null;
        }

        transitionPanel.SetActive(false); 
    }

    private IEnumerator LevelTransitionCoroutine(string sceneToLoad)
    {
        levelText.text = sceneToLoad.Contains("Level") ? sceneToLoad : "Congratulations!";

        audioMixer.GetFloat("MyExposedParam", out previousVolume);

        audioMixer.SetFloat("MyExposedParam", -80f);  

        transitionPanel.SetActive(true);
        canvasGroup.alpha = 0f; 

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / transitionDuration;
            yield return null;
        }

        yield return new WaitForSeconds(transitionDuration);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        audioMixer.SetFloat("MyExposedParam", previousVolume);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / transitionDuration;
            yield return null;
        }

        transitionPanel.SetActive(false); 
    }
}
