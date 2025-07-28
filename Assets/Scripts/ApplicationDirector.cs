using ColorQuiz.SaveData;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace ColorQuiz
{
    public static class ApplicationDirector
    {
        public static async UniTask Quit(HighScores highScore)
        {
            Debug.Log("ゲームを終了します");
            await SaveDataLoader.SaveDataAsync(StatsWatcher.CreateSaveData(), highScore);
            // ゲームを終了させる機能
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; //ゲームプレイ終了
#else
                Application.Quit(); //ゲームプレイ終了
#endif
        }
    }
}