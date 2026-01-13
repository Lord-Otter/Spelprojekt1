using System;

[Serializable]
public class LeaderboardEntry
{
    public string initials;
    public int score;

    public LeaderboardEntry(string initials, int score)
    {
        this.initials = initials;
        this.score = score;
    }
}
