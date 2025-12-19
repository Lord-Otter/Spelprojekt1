using Spelprojekt1;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        PlayerData.Instance.ResetRun();
    }
}
