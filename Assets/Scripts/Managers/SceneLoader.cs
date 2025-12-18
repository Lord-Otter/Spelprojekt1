using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private List<string> randomScenes = new List<string>();

    public void LoadGameScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadRandomGameScene()
    {
        string sceneToLoad = randomScenes[Random.Range(0, randomScenes.Count)];
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}