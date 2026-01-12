using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit Button Pressed!"); // This let's you know it works in the editor
        Application.Quit(); // This closes the actual game build
    }
}