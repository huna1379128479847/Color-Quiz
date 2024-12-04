using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorQuiz
{
    public class TutorialMouseClick : MonoBehaviour
    {
        private const float TUTORIAL_TIME = 12;
        [SerializeField] private Image _image;
        private float _dontControlTime;
        private bool ShouldBeExpose() => _dontControlTime >= TUTORIAL_TIME;
        private int _currentParetIdx = 0;
        private Coroutine _imageCoroutine;

        private void Update()
        {
            _dontControlTime += Time.deltaTime;

            if (ShouldBeExpose())
            {
                _image.gameObject.SetActive(true);
                if (_imageCoroutine == null) // コルーチンが開始されていない場合のみ実行
                {
                    _imageCoroutine = StartCoroutine(ImageToParetTransform());
                }
            }
            else
            {
                _image.gameObject.SetActive(false);
                if (_imageCoroutine != null) // コルーチンが動作している場合停止
                {
                    StopCoroutine(_imageCoroutine);
                    _imageCoroutine = null;
                }
            }

            if (Input.anyKey)
            {
                _dontControlTime = 0;
            }
        }


        public IEnumerator ImageToParetTransform()
        {
            List<Transform> transforms = new List<Transform>();
            foreach (var iten in Director.instance.colorPallets)
            {
                transforms.Add(transform);
            }
            while (true)
            {
                if (!ShouldBeExpose())
                {
                    _image.gameObject.SetActive(false);
                    yield break;
                }
                _image.transform.position = transforms[_currentParetIdx].position;
                _currentParetIdx++;
                if (_currentParetIdx >= Director.instance.colorPallets.Count)
                {
                    _currentParetIdx = 0;
                }
                yield return new WaitForSeconds(3);
            }
        }

    }
}
