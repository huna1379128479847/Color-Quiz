using UnityEngine;
using UnityEngine.Events;

namespace HighElixir.Utilities
{
    public enum PauseMode
    {
        None,
        Normal,          // 通常のポーズ
        RejectPlayerInput, // プレイヤーの入力を拒否
        PerfectFreeze     // 完全停止（内部処理を除く）
    }

    public static class GameSpeedHelper
    {
        private static float _beforePauseTimeScale;
        private static float _maxTimeScale = 5f;

        public static UnityEvent OnPause { get; private set; }
        public static UnityEvent OnPlay { get; private set; }

        static GameSpeedHelper()
        {
            OnPause = new UnityEvent();
            OnPlay = new UnityEvent();
        }

        public static bool IsPause => Time.timeScale == 0;

        public static PauseMode PauseMode { get; private set; } = PauseMode.None;

        public static void Pause(PauseMode mode = PauseMode.Normal)
        {
            if (IsPause && PauseMode == mode) return;

            _beforePauseTimeScale = Time.timeScale;
            PauseMode = mode;

            switch (mode)
            {
                case PauseMode.Normal:
                    Time.timeScale = 0;
                    break;
                case PauseMode.RejectPlayerInput:
                    Time.timeScale = 0;
                    // 入力拒否ロジックをここに追加
                    break;
                case PauseMode.PerfectFreeze:
                    Time.timeScale = 0;
                    // 完全停止用の処理をここに追加
                    break;
            }

            OnPause?.Invoke();
        }

        public static void Play()
        {
            if (!IsPause) return;

            Time.timeScale = _beforePauseTimeScale;
            OnPlay?.Invoke();
            PauseMode = PauseMode.None;
        }

        public static void TimeLeap(float targetTime)
        {
            if (IsPause)
            {
                Debug.LogWarning("ポーズ中はゲーム速度を変更できません。");
                return;
            }
            Time.timeScale = Mathf.Clamp(targetTime, 0.01f, _maxTimeScale);
        }

        public static void SetMaxTimeScale(float max)
        {
            _maxTimeScale = Mathf.Max(0, max);
        }
    }
}
