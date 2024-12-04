namespace ColorQuiz
{
    public static class StatsWatcher
    {
        // 代表的な統計データの変数
        public static int TotalGamesPlayed { get; set; } = 0;           // 総プレイ回数
        public static int HighScore { get; set; } = 0;                  // 最高スコア
        public static int TotalScore { get; set; } = 0;
        public static int TotalCorrectAnswers { get; set; } = 0;        // 正解数の合計
        public static int TotalIncorrectAnswers { get; set; } = 0;      // 不正解数の合計
        public static float TotalTimePlayed { get; set; } = 0;            // プレイ時間（秒）
        public static int TotalResets { get; set; } = 0;                // リセット回数
        public static int TotalClick { get; set; } = 0;                // 総クリック回数

        // 連続記録用の変数
        public static int CurrentStreakCorrect { get; private set; } = 0;  // 現在の連続正解数
        public static int MaxStreakCorrect { get; private set; } = 0;      // 最大連続正解数
        public static int CurrentStreakIncorrect { get; private set; } = 0;// 現在の連続不正解数
        public static int MaxStreakIncorrect { get; private set; } = 0;    // 最大連続不正解数
        public static int CurrentGameClick { get; private set; } = 0;
        public static int MaxGameClick { get; private set; } = 0;

        // 統計データの更新メソッド
        public static void RecordGamePlayed()
        {
            TotalGamesPlayed++;
        }

        public static void UpdateHighScore(int newScore)
        {
            TotalScore += newScore;
            if (newScore > HighScore)
            {
                HighScore = newScore;
            }
        }
        public static void RecordClick()
        {
            CurrentGameClick++;
            TotalClick++;
        }
        public static void ResetClick()
        {
            if (CurrentGameClick > MaxGameClick)
            {
                MaxGameClick = CurrentGameClick;
            }
            CurrentGameClick = 0;
        }
        public static void RecordCorrectAnswer()
        {
            TotalCorrectAnswers++;
            CurrentStreakCorrect++;
            CurrentStreakIncorrect = 0; // 不正解連続記録をリセット

            if (CurrentStreakCorrect > MaxStreakCorrect)
            {
                MaxStreakCorrect = CurrentStreakCorrect;
            }
        }

        public static void RecordIncorrectAnswer()
        {
            TotalIncorrectAnswers++;
            CurrentStreakIncorrect++;
            CurrentStreakCorrect = 0; // 正解連続記録をリセット

            if (CurrentStreakIncorrect > MaxStreakIncorrect)
            {
                MaxStreakIncorrect = CurrentStreakIncorrect;
            }
        }

        public static void IncrementResetCount()
        {
            TotalResets++;
        }

        public static void AddPlayTime(float seconds)
        {
            TotalTimePlayed += seconds;
        }

        // データリセットメソッド
        public static void ResetStats()
        {
            TotalGamesPlayed = 0;
            HighScore = 0;
            TotalCorrectAnswers = 0;
            TotalIncorrectAnswers = 0;
            TotalTimePlayed = 0;
            TotalResets = 0;
            TotalClick = 0;

            CurrentStreakCorrect = 0;
            MaxStreakCorrect = 0;
            CurrentStreakIncorrect = 0;
            MaxStreakIncorrect = 0;
            CurrentGameClick = 0;
            MaxGameClick = 0;
        }

        public static void SetSaveData(SaveData.SaveData saveData)
        {
            TotalClick = saveData.TotalClick;
            TotalScore = saveData.TotalScore;
            HighScore = saveData.HighScore;
            TotalCorrectAnswers = saveData.TotalCorrectAnswers;
            TotalIncorrectAnswers = saveData.TotalIncorrectAnswers;
            TotalTimePlayed = saveData.TotalTimePlayed;
            TotalResets = saveData.TotalResets;
            
            MaxGameClick = saveData.MaxGameClick;
            MaxStreakCorrect = saveData.MaxStreakCorrect;
            MaxStreakIncorrect = saveData.MaxStreakIncorrect;
        }

        public static SaveData.SaveData CreateSaveData()
        {
            SaveData.SaveData saveData = new SaveData.SaveData()
            {
                HighScore = HighScore,
                TotalGamesPlayed = TotalGamesPlayed,
                TotalCorrectAnswers = TotalCorrectAnswers,
                TotalIncorrectAnswers = TotalIncorrectAnswers,
                TotalClick = TotalClick,
                TotalResets = TotalResets,
                TotalScore = TotalScore,
                TotalTimePlayed = (float)System.Math.Truncate(TotalTimePlayed * 100) / 100,
                MaxGameClick = MaxGameClick,
                MaxStreakCorrect = MaxStreakCorrect,
                MaxStreakIncorrect = MaxStreakIncorrect,
            };
            return saveData;
        }
    }
}
