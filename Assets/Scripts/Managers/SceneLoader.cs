using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    public void LoadGameScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadRandomGameScene(List<string> scenes)
    {
        string sceneToLoad = scenes[Random.Range(0, scenes.Count)];
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}