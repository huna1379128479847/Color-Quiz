using System;
using UnityEngine;
using System.IO;
using ColorQuiz.Achieves;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace ColorQuiz.SaveData
{
    public static class SaveDataLoader
    {
        private static string absolutePathPrefix;
        private static readonly string jsonPath = "SaveData";
        private static readonly string achievementsPath = "Achievements";

        public static bool IsFinishedLoadAchievementData { get; private set; } = false;
        public static bool IsFinishedLoadSaveData { get; private set; } = false;

        public static Achievements Achievements { get; private set; }
        public static bool CanGameStart() => IsFinishedLoadAchievementData && IsFinishedLoadSaveData;

        public static async UniTask LoadData()
        {
            await UniTask.WhenAll(LoadAchievementData(), LoadSaveData(), LoadScoreData());
        }

        private static async UniTask LoadAchievementData()
        {
            string directoryPath = $"{absolutePathPrefix}/{achievementsPath}";
            string filePath = $"{directoryPath}/achievements.json";

            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = await File.ReadAllTextAsync(filePath);
                    Achievements = JsonUtility.FromJson<Achievements>(jsonContent);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load achievement data: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Achievements data file not found, initializing empty data.");

            }
            IsFinishedLoadAchievementData = true;
        }

        private static async UniTask LoadSaveData()
        {
            string directoryPath = $"{absolutePathPrefix}/{jsonPath}";
            string filePath = $"{directoryPath}/saveData.json";

            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = await File.ReadAllTextAsync(filePath);
                    SaveData saveData = JsonUtility.FromJson<SaveData>(jsonContent);
                    StatsWatcher.SetSaveData(saveData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load save data: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Save data file not found, initializing with default values.");
            }
            IsFinishedLoadSaveData = true;
        }

        private static async UniTask LoadScoreData()
        {
            string directoryPath = $"{absolutePathPrefix}/{jsonPath}";
            string filePath = $"{directoryPath}/highScore.json";

            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = await File.ReadAllTextAsync(filePath);
                    HighScore saveData = JsonUtility.FromJson<HighScore>(jsonContent);
                    foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
                    {
                        if (go.TryGetComponent<StatsManager>(out var statsManager))
                            statsManager.SetHighScore(saveData);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load save data: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Save data file not found, initializing with default values.");
            }
            IsFinishedLoadSaveData = true;
        }

        public static async UniTask SaveDataAsync(SaveData data, HighScore highScore)
        {
            string directoryPath = $"{absolutePathPrefix}/{jsonPath}";
            string filePath = $"{directoryPath}/saveData.json";
            string scorePath = $"{directoryPath}/highScore.json";

            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            string jsonContent = JsonUtility.ToJson(data);
            string highScoreContent = JsonUtility.ToJson(highScore);
            try
            {
                await File.WriteAllTextAsync(filePath, jsonContent);
                await File.WriteAllTextAsync(scorePath, highScoreContent);
                Debug.Log("Save data successfully saved.");
                Debug.Log("To:" + directoryPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data: {ex.Message}");
            }
        }

        public static async UniTask Invoke()
        {

            absolutePathPrefix = Application.persistentDataPath;
            try
            {
                await LoadData();
                if (CanGameStart())
                {
                    Debug.Log("Game data successfully loaded. Ready to start the game.");
                }
                else
                {
                    Debug.LogWarning("Data loading incomplete. Some data might be missing.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during data loading in Awake: {ex.Message}");
            }
        }
    }
}
