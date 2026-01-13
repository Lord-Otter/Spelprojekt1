using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardManager : MonoBehaviour
{
    private const string LeaderboardKey = "LEADERBOARD";
    private const int MaxEntries = 10;

    public static List<LeaderboardEntry> LoadLeaderboard()
    {
        if (!PlayerPrefs.HasKey(LeaderboardKey))
            return new List<LeaderboardEntry>();

        string json = PlayerPrefs.GetString(LeaderboardKey);
        LeaderboardWrapper wrapper = JsonUtility.FromJson<LeaderboardWrapper>(json);
        return wrapper.entries;
    }

    public static void SaveScore(string initials, int score)
    {
        List<LeaderboardEntry> entries = LoadLeaderboard();

        entries.Add(new LeaderboardEntry(initials, score));

        entries = entries.OrderByDescending(e => e.score).Take(MaxEntries).ToList();

        SaveLeaderboard(entries);
    }

    private static void SaveLeaderboard(List<LeaderboardEntry> entries)
    {
        LeaderboardWrapper wrapper = new LeaderboardWrapper { entries = entries };

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class LeaderboardWrapper
    {
        public List<LeaderboardEntry> entries;
    }
}
