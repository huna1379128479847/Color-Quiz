using System;
using System.Collections.Generic;

namespace ColorQuiz.SaveData
{
    [Serializable]
    public class SaveData
    {
        public int HighScore;
        public int TotalGamesPlayed;
        public int TotalCorrectAnswers;
        public int TotalIncorrectAnswers;
        public float TotalTimePlayed;
        public int TotalResets;
        public int TotalClick;
        public int TotalScore;
        public int MaxStreakCorrect;
        public int MaxStreakIncorrect;
        public int MaxGameClick;
        public List<string> AnlockedAchieves;
        public List<ColorParetStats> ColorParetStats;
    }

    [Serializable]
    public class ColorParetStats
    {
        public string ParetName;
        public int HighScore;
        public int TotalGamesPlayed;
        public int TotalCorrectAnswers;
        public int TotalIncorrectAnswers;
        public float TotalTimePlayed;
        public int TotalResets;
        public int TotalClick;
        public int TotalScore;
        public int MaxStreakCorrect;
        public int MaxStreakIncorrect;
        public int MaxGameClick;
        public List<ColorClickCount> ColorStats;
    }
    [Serializable]
    public class ColorClickCount
    {
        public string ColorName;
        public int TotalClick;
    }
}
