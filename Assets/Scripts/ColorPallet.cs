using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

namespace ColorQuiz
{
    [DefaultExecutionOrder(-1)]
    public class ColorPallet : MonoBehaviour, IPointerClickHandler
    {
        public static Color answer;
        public Image image;
        private AudioSource _audioSource;

        [SerializeField] private ColorData _colorData;
        public void OnPointerClick(PointerEventData pointerData)
        {
            _audioSource?.Play();

            if (LifeCounter.instance.IsGameOver()) { return; }
            if (gameObject.name == "Ans") { return; }
            Director.instance.ClickTiles(_colorData, answer == _colorData.color);

            //Debug.LogError("error");
        }
        /// <returns>Color in ColorData or dafault.</returns>
        public Color SetColor(ColorData changeColer)
        {
            if (changeColer.color == null || image == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name);
                if (changeColer.color == null)
                {
                    sb.AppendLine(":カラーが設定されていません。");
                }
                if (image == null)
                {
                    sb.AppendLine(":イメージがアタッチされていません。");
                }
                Debug.LogError(sb.ToString());
                return default;
            }
            if (changeColer == _colorData)
                return changeColer.color;
            _colorData = changeColer;
            if (_colorData.color.a < 1f)
            {
                _colorData.color.a = 1f; // Ensure the alpha is set to fully opaque
            }
            image.color = _colorData.color;
            return _colorData.color;
        }
        public ColorData GetColor()
        {
            return _colorData;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            image = GetComponent<Image>();
        }
    }
}