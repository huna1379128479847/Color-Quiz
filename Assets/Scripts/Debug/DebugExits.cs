using ColorQuiz.SaveData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ColorQuiz.Debugs
{
    public class DebugExits : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnclickEvent);
        }


        async void OnclickEvent()
        {
            await Director.instance.GetComponent<SaveDataLoader>().SaveDataAsync(new SaveData.SaveData(), new HighScore());
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; //ゲームプレイ終了
#else
                Application.Quit(); //ゲームプレイ終了
#endif
        }
    }
}
