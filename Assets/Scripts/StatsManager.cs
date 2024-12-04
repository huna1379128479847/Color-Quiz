using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ColorQuiz.SaveData;

namespace ColorQuiz
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] TMP_Text _currentScoreObject;
        [SerializeField] TMP_Text _ColorNameObject;
        [SerializeField] List<TMP_Text> scores;
        public List<int> highScore = new List<int>();
        private int _currentScore = 0;
        private void Start()
        {
            highScore.Add(0);
            highScore.Add(0);
            highScore.Add(0);
            SortScoreList();
        }

        private void UpdateHighScoreDisplay()
        {
            for (int n = 0; n < scores.Count; n++)
            {
                if (n >= highScore.Count)
                {
                    scores[n].SetText($"{n + 1}st : 000");
                }
                else
                {
                    scores[n].SetText($"{n + 1}st : {highScore[n]:D3}");
                }
            }
        }

        public void SetHighScore(HighScore highScore)
        {
            this.highScore.Add(highScore.First);
            this.highScore.Add(highScore.Second);
            this.highScore.Add(highScore.Third);
            SortScoreList();
        }
        public void SetHighScore()
        {
            StatsWatcher.UpdateHighScore(_currentScore);
            highScore.Add(_currentScore);
            SortScoreList();
        }
        public void SortScoreList()
        {
            highScore.Sort((a, b) => b - a);
            if (highScore.Count > 3)
            {
                highScore.RemoveAt(highScore.Count - 1);
            }
            UpdateHighScoreDisplay();
        }
        public void SetScore()
        {
            _currentScoreObject.SetText($"Score : {_currentScore.ToString("D3")}");
        }
        public void SetColorName(string colorName)
        {
            _ColorNameObject.SetText(colorName);
        }

        public void AddScore(int score)
        {
            _currentScore = Mathf.Max(_currentScore + score, 0);
            SetScore();
        }
        public void SetScore(int score)
        {
            _currentScore = score;
            SetScore();
        }
        public void ResetScore()
        {
            _currentScore = 0;
            SetScore();
        }
    }
}