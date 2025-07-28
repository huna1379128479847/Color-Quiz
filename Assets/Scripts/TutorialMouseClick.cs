using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using HighElixir.Utilities;

namespace ColorQuiz
{
    public class TutorialMouseClick : MonoBehaviour
    {
        private static readonly float TUTORIAL_TIME = 12;


        [SerializeField] private Image _image;
        [PropertyTooltip("パレットからずらす座標"), SerializeField] private Vector2 _positionOffset = new Vector2(0, 100);

        private float _dontControlTime;
        private bool _onTask = false;
        private bool ShouldBeExpose => _dontControlTime >= TUTORIAL_TIME;

        public async UniTask ImageToParetTransform()
        {
            _onTask = true;
            _image.gameObject.SetActive(true);
            List<Transform> transforms = new List<Transform>(Director.instance.colorPallets.ConvertAll(p => p.transform));
            LoopableInt idx = new(0, transforms.Count, 0);

            // 1秒ごとに次のパレットへ移動
            while (ShouldBeExpose)
            {
                _image.transform.position = transforms[idx.Value].position + (Vector3)_positionOffset;
                idx.Value++;
                await UniTask.Delay(1000); // 1秒待機
            }
            _image.gameObject.SetActive(false);
            _onTask = false;
        }

        private void Awake()
        {
            _image.gameObject.SetActive(false);
            InputSystem.onAnyButtonPress.Subscribe(_ => _dontControlTime = 0).AddTo(this);
            this.FixedUpdateAsObservable()
                .Where(_ => !ShouldBeExpose)
                .Subscribe(_ => _dontControlTime = Mathf.Min(_dontControlTime + Time.fixedDeltaTime, TUTORIAL_TIME))
                .AddTo(this);
            this.UpdateAsObservable()
                .Where(_ => ShouldBeExpose && !_onTask)
                .Subscribe(unused => {
                    _ = ImageToParetTransform();
                })
                .AddTo(this);
        }
    }
}
