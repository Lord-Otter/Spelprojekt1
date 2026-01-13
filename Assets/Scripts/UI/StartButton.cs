using Spelprojekt1;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public void StartGame()
    {
        PlayerData.Instance.ResetRun();
        EnemyRunData.Instance.ResetRun();
    }

    public void ToggleOnOff()
    {
        if (target == null)
            return;

        target.SetActive(!target.activeSelf);
    }
}
