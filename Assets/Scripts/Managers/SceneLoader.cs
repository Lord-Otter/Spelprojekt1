using System.Collections.Generic;
using UnityEngine; using UnityEngine.SceneManagement; 
public class SceneLoader : MonoBehaviour 
{ 
    //[SerializeField] private Canvas blackScreenCanvas; 
    //private Image blackScreen; 
    [SerializeField] private List<string> randomScenes = new List<string>(); 

    private void Start() 
    { 
        //Canvas canvas = Instantiate(blackScreenCanvas, transform); 
        //blackScreen = canvas.GetComponent<Image>(); 
    } 

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