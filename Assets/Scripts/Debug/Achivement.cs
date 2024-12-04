using ColorQuiz.Achieves;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ColorQuiz.SaveData;

namespace ColorQuiz.Debugs
{
    public class Achievement : MonoBehaviour
    {
        [SerializeField] private List<TMP_InputField> _inputFields = new List<TMP_InputField>();
        [SerializeField] private Button _confirm;
        [SerializeField] private Button _cancel;
        [SerializeField] private Button _exit;
        [SerializeField] private List<string> _achievementTitles = new List<string>();
        [SerializeField] private Achievements _achievements;
        private Achieves.Achievement _achievement;
        private SaveDataLoader _saveDataLoader;
        private bool finishedInit = false;

        private void Awake()
        {
            _saveDataLoader = GetComponent<SaveDataLoader>();
            _achievements = new Achievements()
            {
                achivements = new List<Achieves.Achievement>()
            };
            _achievement = new Achieves.Achievement();

            // Confirmボタンにリスナーを追加
            if (_confirm != null)
            {
                _confirm.onClick.AddListener(Confirm);
                TMP_Text confirmText = _confirm.GetComponentInChildren<TMP_Text>();
                if (confirmText != null) confirmText.SetText("Confirm");
            }

            // Cancelボタンにリスナーを追加
            if (_cancel != null)
            {
                _cancel.onClick.AddListener(Cancel);
                TMP_Text cancelText = _cancel.GetComponentInChildren<TMP_Text>();
                if (cancelText != null) cancelText.SetText("Cancel");
            }

            // Exitボタンにリスナーを追加
            if (_exit != null)
            {
                _exit.onClick.AddListener(Exit);
                TMP_Text exitText = _exit.GetComponentInChildren<TMP_Text>();
                if (exitText != null) exitText.SetText("Exit");
            }

            // 各入力フィールドにタイトルを設定
            for (int i = 0; i < _inputFields.Count; i++)
            {
                if (i >= _achievementTitles.Count) { break; }
                TMP_Text titleText = _inputFields[i].GetComponentInChildren<TMP_Text>();
                if (titleText != null)
                {
                    titleText.SetText(_achievementTitles[i]);
                }
                else
                {
                    Debug.LogWarning($"Warning: No TMP_Text component found in input field at index {i}");
                }
            }
        }
        private void Update()
        {
            if (_saveDataLoader.CanGameStart() && !finishedInit)
            {
                _achievements = _saveDataLoader.Achievements;
                finishedInit = true;
                if (_achievements == null)
                {
                    _achievements = new Achievements();
                }
            }
        }
        public void Exit()
        {
            SaveDataAsync();
        }

        public void SaveDataAsync()
        {
            string directoryPath = $"{Application.persistentDataPath}/Achievements";
            string filePath = $"{directoryPath}/achievements.json";
            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string jsonContent = JsonUtility.ToJson(_achievements);

            try
            {
                File.WriteAllTextAsync(filePath, jsonContent);
                Debug.Log("Save data successfully saved.");
                Debug.Log("To:" + filePath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data: {ex.Message}");
            }
        }
        private void Cancel()
        {
            _achievement = new Achieves.Achievement();
        }

        private void Confirm()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var field in _inputFields)
            {
                if (string.IsNullOrWhiteSpace(field.text))
                {
                    Debug.LogWarning("All fields must be filled in.");
                    return;
                }

                TMP_Text label = field.GetComponentInChildren<TMP_Text>();
                if (label == null) continue;

                // 入力フィールドのタイトルに基づいて適切なプロパティに設定
                switch (label.text)
                {
                    case "UniqueName":
                        _achievement.UniqueName = field.text;
                        sb.AppendLine($"UniqueName:{field.text}");
                        break;

                    case "Name":
                        _achievement.Name = field.text;
                        sb.AppendLine($"Name:{field.text}");
                        break;

                    case "Description":
                        _achievement.Description = field.text;
                        sb.AppendLine($"Description:{field.text}");
                        break;

                    case "IconPath":
                        _achievement.IconPath = field.text;
                        sb.AppendLine($"IconPath:{field.text}");
                        break;

                    case "TargetStats":
                        _achievement.TargetStats = field.text;
                        sb.AppendLine($"TargetStats:{field.text}");
                        break;

                    case "Goal":
                        if (int.TryParse(field.text, out int goal))
                        {
                            _achievement.Goal = goal;
                            sb.AppendLine($"Goal:{field.text}");
                        }
                        else Debug.LogWarning("Invalid Goal input. Please enter a valid integer.");
                        break;

                    default:
                        Debug.LogWarning($"Unknown field: {label.text}");
                        break;
                }

            }
            Debug.Log(sb.ToString());
            // 実績リストに追加
            _achievements.achivements.Add(_achievement);
            _achievement = new Achieves.Achievement();
            Debug.Log("Achievement added successfully.");
        }
    }
}
