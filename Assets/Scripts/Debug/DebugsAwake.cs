using UnityEngine;
using UnityEngine.UI;

namespace ColorQuiz.Debugs
{
    public class DebugsAwake : MonoBehaviour
    {
        [SerializeField] Button _debugExitsButton;

        private void Awake()
        {
#if UNITY_EDITOR
            _debugExitsButton.gameObject.SetActive(true);
#endif
        }
    }
}
