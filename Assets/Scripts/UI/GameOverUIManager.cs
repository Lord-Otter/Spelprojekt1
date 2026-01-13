using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text enemiesKilledText;
    [SerializeField] private TMP_Text wavesClearedText;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Score Submit")]
    [SerializeField] private TMP_InputField initialsInput;
    [SerializeField] private Button submitButton;

    private bool submitted;

    private void Start()
    {
        EnemyRunData runData = EnemyRunData.Instance;

        timeText.text = $"Time: {runData.GetFormattedTime()}";
        enemiesKilledText.text = $"Enemies Killed: {runData.enemiesKilled}";
        wavesClearedText.text = $"Waves Cleared: {runData.totalWavesCleared}";
        finalScoreText.text = $"Score: {runData.score}";

        initialsInput.characterLimit = 3;
        initialsInput.onValueChanged.AddListener(ValidateInput);

        submitButton.interactable = false;
        submitButton.onClick.AddListener(SubmitScore);
    }

    private void ValidateInput(string input)
    {
        submitButton.interactable = !string.IsNullOrWhiteSpace(input) && !submitted;
    }

    private void SubmitScore()
    {
        if(submitted)
            return;

        string initials = initialsInput.text.ToUpper();
        int score = EnemyRunData.Instance.score;

        LeaderboardManager.SaveScore(initials, score);

        submitted = true;
        submitButton.interactable = false;
        initialsInput.interactable = false;

        Debug.Log("Submitted?" + submitted);
    }
}
