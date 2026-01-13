using TMPro;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text leaderboardText;

    private void Start()
    {
        List<LeaderboardEntry> entries = LeaderboardManager.LoadLeaderboard();
        StringBuilder sb = new StringBuilder();

        int rank = 1;
        foreach(LeaderboardEntry entry in entries)
        {
            sb.AppendLine($"{rank}. {entry.initials} - {entry.score}");
            rank++;
        }

        leaderboardText.text = sb.ToString();
    }
}
