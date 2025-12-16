using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenu;

    private void Awake()
    {
        Instantiate(pauseMenu);
    }
}
