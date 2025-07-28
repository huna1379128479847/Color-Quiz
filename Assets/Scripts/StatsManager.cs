using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ColorQuiz.SaveData;
using System.Linq;

namespace ColorQuiz
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] TMP_Text _currentScoreObject;
        [SerializeField] TMP_Text _ColorNameObject;
        [SerializeField] List<TMP_Text> scores;
        public HighScores highScore = new();
        private int _currentScore = 0;
        public HighScore Current
        {
            get
            {
                var existingHighScore = highScore.Scores.FirstOrDefault(item => item.ColorName == Director.instance.CurrentColorName);
                if (existingHighScore != null)
                {
                    return existingHighScore;
                }
                else
                {
                    var newHighScore = new HighScore { ColorName = Director.instance.CurrentColorName };
                    highScore.Scores.Add(newHighScore);
                    return newHighScore;
                }
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = Mathf.Max(value, 0);
                UpdateUI();
            }
        }
        public void SetHighScore(HighScores highScore)
        {
            this.highScore = highScore;
            SortHighScore();
            UpdateUI();
        }
        public void SetHighScore()
        {
            StatsWatcher.UpdateHighScore(_currentScore);
            SortHighScore();
            UpdateUI();
        }
        public HighScores SortHighScore()
        {
            var tmp = Current.ToList();
            tmp.Add(_currentScore);
            tmp.Sort((a, b) => b - a);
            if (tmp.Count > 3)
            {
                tmp.RemoveAt(tmp.Count - 1);
            }
            for (int i = 0; i < tmp.Count; i++)
            {
                Current[i] = tmp[i];
            }
            return highScore;
        }

        public void ResetScore()
        {
            _currentScore = 0;
            UpdateUI();
        }
        public void UpdateUI()
        {
            _currentScoreObject.SetText($"Score : {_currentScore.ToString("D3")}");
            UpdateHighScoreDisplay();
        }

        private void UpdateHighScoreDisplay()
        {
            for (int n = 0; n < scores.Count; n++)
                scores[n].SetText($"{n + 1}st : {Current[n]:D3}");
        }
    }
}