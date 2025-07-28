using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using HighElixir.Utilities;
using DG.Tweening;

namespace ColorQuiz
{
    public class MouseClickTutor : MonoBehaviour
    {
        private static readonly float TUTORIAL_TIME = 12;


        [SerializeField] private Image _image;
        [PropertyTooltip("パレットからずらす座標"), SerializeField] private Vector2 _positionOffset = new Vector2(0, 100);

        private float _dontControlTime;
        private bool _onTask = false;

        // チュートリアルが実行されるべきかどうかを判断するためのプロパティ
        private bool ShouldBeExpose => _dontControlTime >= TUTORIAL_TIME;

        public async UniTask DoMove()
        {
            // ロック
            _onTask = true;
            // 画像を表示
            _image.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            // パレットのTransformを取得し、ループ可能なインデックスを作成
            List<Transform> transforms = new List<Transform>(Director.instance.colorPallets.ConvertAll(p => p.transform));
            // ループ可能なインデックスを作成
            LoopableInt idx =
                new LoopableInt(0)
                .SetMin(0)
                .SetMax(transforms.Count - 1);

            // 一定時間ごとに次のパレットへ移動
            while (ShouldBeExpose)
            {
                _image.transform.position = transforms[idx.Value].position + (Vector3)_positionOffset;
                sequence = _image.transform.DOJump(_image.transform.position, 50, 1, 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
                idx.Value++;
                await UniTask.Delay(2500); // 2.5秒待機
            }
            // タスクが完了したら、画像を非表示にする
            sequence.Kill();
            _image.gameObject.SetActive(false);
            _onTask = false;
        }

        private void Awake()
        {
            // 初期化
            _image.gameObject.SetActive(false);

            // 画面のクリックで時間をリセット
            InputSystem.onAnyButtonPress.Subscribe(_ => _dontControlTime = 0).AddTo(this);

            // チュートリアル条件未達成時、時間をカウントアップ
            this.FixedUpdateAsObservable()
                .Where(_ => !ShouldBeExpose)
                .Subscribe(_ => _dontControlTime = Mathf.Min(_dontControlTime + Time.fixedDeltaTime, TUTORIAL_TIME))
                .AddTo(this);

            // タイマーが経過したらDoMoveを実行
            this.UpdateAsObservable()
                .Where(_ => ShouldBeExpose && !_onTask)
                .Subscribe(unused =>
                {
                    _ = DoMove();
                })
                .AddTo(this);
        }
    }
}
