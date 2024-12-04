using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

namespace ColorQuiz
{
    public class ColorPallet : MonoBehaviour, IPointerClickHandler
    {
       public  Image image;
        private AudioSource _audioSource;
        
        private ColorData _colorData;
        // Start is called before the first frame update
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            image = GetComponent<Image>();
            if (gameObject.name == "Ans") 
                return;
            Director.instance.colorPallets.Add(this);
        }
        public void OnPointerClick(PointerEventData pointerData)
        {
            _audioSource?.Play();
            Debug.Log($"{Time.time} This color is {ColorUtility.ToHtmlStringRGB(_colorData.color)}.");
            Director.instance.ClickTiles(this);

            //Debug.LogError("error");
        }
        public void SetColor(ColorData changeColer)
        {
            if (changeColer == _colorData)
                return;
            if (changeColer.color == null || image == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name + "でエラー発生");
                if (changeColer.color == null)
                {
                    sb.AppendLine(":カラーが設定されていません。");
                }
                if (image == null)
                {
                    sb.AppendLine(":イメージがアタッチされていません。");
                }
                Debug.LogError(sb.ToString());
                return;
            }
            image.color = changeColer.color;
            _colorData = changeColer;
        }
        public ColorData GetColor()
        {
            return _colorData;
        }
    }
}