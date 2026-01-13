using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour 
{ 
    [SerializeField] private Canvas blackScreenCanvas;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float preFadeDelay;
    [SerializeField] private float postFadeDelay;
    private Image blackScreen; 
    [SerializeField] private List<string> randomScenes = new List<string>(); 

    private void Start() 
    { 
        Canvas canvas = Instantiate(blackScreenCanvas, transform); 
        blackScreen = canvas.GetComponentInChildren<Image>(); 

        SetAlpha(1f);
        StartCoroutine(FadeIn());
    } 

    public void LoadGameScene(string sceneToLoad) 
    { 
        StartCoroutine(FadeOutAndLoad(sceneToLoad));
    } 
    
    public void LoadRandomGameScene() 
    { 
        if(randomScenes.Count <= 1)
        {
            Debug.Log("There are no game scenes to load");
            return;
        }
            
            
        Scene currentScene = SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;

        string sceneToLoad;

        do
        {
            sceneToLoad = randomScenes[Random.Range(0, randomScenes.Count)];
        }
        while (sceneToLoad == currentSceneName);

        StartCoroutine(FadeOutAndLoad(sceneToLoad));
    } 
    
    public void OnQuit() 
    { 
        StartCoroutine(FadeOutAndQuit());
    } 

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(preFadeDelay);
        yield return Fade(1f, 0f);
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(postFadeDelay);
        SceneManager.LoadScene(sceneName);

        yield return Fade(1f, 0f); // Remove black screen incase the game doesn't load next scene.
    }

    private IEnumerator FadeOutAndQuit()
    {
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(postFadeDelay);
        Application.Quit();

        yield return new WaitForSeconds(1);
        yield return Fade(1f, 0f); // Remove black screen incase the game doesn't quit.
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(endAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }
}