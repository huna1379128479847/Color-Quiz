using ColorQuiz.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ColorQuiz
{
    public class StatsStringBuilder : MonoBehaviour
    {
        [SerializeField]private TMP_Text _statsTexts;
        private StringBuilder sb;

        private string stringConcat(string left, int right) => $"{left}：{right.ToString()}";

        protected void Update()
        {
            sb = new StringBuilder();
            sb.AppendLine(stringConcat("累計クリック数", StatsWatcher.TotalClick));
            sb.AppendLine(stringConcat("累計プレイ回数", StatsWatcher.TotalGamesPlayed));
            sb.AppendLine(stringConcat("累計リセット回数", StatsWatcher.TotalResets));
            sb.AppendLine(stringConcat("最高スコア", StatsWatcher.HighScore));
            sb.AppendLine(stringConcat("累計スコア", StatsWatcher.TotalScore));
            sb.AppendLine(stringConcat("正解数の合計", StatsWatcher.TotalCorrectAnswers));
            sb.AppendLine(stringConcat("不正解数の合計", StatsWatcher.TotalIncorrectAnswers));
            sb.AppendLine(stringConcat("総プレイ時間 (秒)", (int)StatsWatcher.TotalTimePlayed));
            sb.AppendLine(stringConcat("現在の連続正解数", StatsWatcher.CurrentStreakCorrect));
            sb.AppendLine(stringConcat("最大連続正解数", StatsWatcher.MaxStreakCorrect));
            sb.AppendLine(stringConcat("現在の連続不正解数", StatsWatcher.CurrentStreakIncorrect));
            sb.AppendLine(stringConcat("最大連続不正解数", StatsWatcher.MaxStreakIncorrect));
            sb.AppendLine(stringConcat("現在のゲームクリック数", StatsWatcher.CurrentGameClick));
            sb.AppendLine(stringConcat("最大ゲームクリック数", StatsWatcher.MaxGameClick));

            _statsTexts.SetText(sb.ToString());
        }

    }
}
