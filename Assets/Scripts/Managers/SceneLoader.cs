using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public void LoadGameScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}