using System;

namespace HighElixir.Utilities
{
    public class LoopableInt
    {
        private class Disposable : IDisposable
        {
            private Action<int> _onLoop;
            private Action<int> _action;
            public Disposable(Action<int> onLoop, Action<int> action)
            {
                _onLoop = onLoop;
                _action = action;
            }
            public void Dispose()
            {
                _onLoop -= _action;
            }
        }
        private int value;
        private int _minValue = int.MinValue;
        private int _maxValue = int.MaxValue;

        private int _direction = 0; // 1 for increasing, -1 for decreasing
        private Action<int> _onLoop;
        public int Direction
        {
            get => _direction;
            private set => _direction = (value == 0) ? 0 : value / Math.Abs(value);
        }
        public int Value
        {
            get => value;
            set
            {
                int oldValue = this.value;
                int rangeSize = _maxValue - _minValue + 1;

                if (rangeSize <= 0)
                {
                    this.value = _minValue;
                    return;
                }

                int mod = (value - _minValue) % rangeSize;
                if (mod < 0) mod += rangeSize;

                int newValue = _minValue + mod;

                // Direction の更新
                int diff = newValue - oldValue;
                Direction = diff;

                // 値の更新
                this.value = newValue;

                // ループ発動条件：入力と出力の差が rangeSize を超えていたら
                if (Math.Abs(value - oldValue) >= rangeSize)
                {
                    _onLoop?.Invoke(Direction);
                }
            }
        }
        public int MinValue => _minValue;
        public int MaxValue => _maxValue;
        public LoopableInt(int initialValue)
        {
            Value = initialValue;
        }
        public LoopableInt(int minValue, int maxValue, int initialValue = 0)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            Value = initialValue;
        }
        public LoopableInt SetMax(int maxValue)
        {
            _maxValue = maxValue;
            if (Value > maxValue)
                Value = Value;
            return this;
        }
        public LoopableInt SetMin(int minValue)
        {
            _minValue = minValue;
            if (Value < minValue)
                Value = minValue;
            return this;
        }
        public IDisposable Subscribe(Action<int> onLoop)
        {
            _onLoop += onLoop;
            return new Disposable(_onLoop, onLoop);
        }
    }
}