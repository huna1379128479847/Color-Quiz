using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace HighElixir.Utilities
{
    public class SceneChanger : IDisposable, ISceneHandler
    {
        // フィールド
        private Scene fromScene;
        private Scene currentScene;
        private SceneManagerArgumants args;
        // イベント デリゲート
        public UnityEvent OnSceneChanged { get; private set; }

        // プロパティ
        public Scene FromScene => fromScene;
        public Scene CurrentScene => currentScene;

        // コンストラクタ
        public SceneChanger(Scene scene)
        {
            currentScene = scene;
            OnSceneChanged = new UnityEvent();

            // シーンがロードされた時にcurrentSceneを更新
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // シーンロード完了後に呼ばれる
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene;
            GameObject[] gameObjects = currentScene.GetRootGameObjects();
            if (gameObjects.Length > 0)
            {
                foreach(var obj in gameObjects)
                {
                    if (obj.TryGetComponent(out ISceneManagement manager))
                    {
                        manager.Execute(args);
                    }
                }
            }
            OnSceneChanged.Invoke();  // シーン変更イベント発火
        }

        // シーン変更メソッド
        public bool SceneChange(string toSceneName, SceneManagerArgumants args, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            fromScene = currentScene;
            this.args = args;
            try
            {
                SceneManager.LoadScene(toSceneName, sceneMode);
                // currentSceneの更新はOnSceneLoadedで行う
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"シーンが見つかりませんでした: {toSceneName}. エラー: {ex.Message}");
                return false;
            }
        }

        public bool SceneChange(int toSceneIdx, SceneManagerArgumants args, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            fromScene = currentScene;
            this.args = args;
            try
            {
                SceneManager.LoadScene(toSceneIdx, sceneMode);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"シーンが見つかりませんでした: {toSceneIdx}. エラー: {ex.Message}");
                return false;
            }
        }
    }
}