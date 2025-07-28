using System;

namespace HighElixir.Utilities
{
    public class LoopableFloat
    {
        private class Disposable : IDisposable
        {
            private Action<float> _onLoop;
            private Action<float> _action;
            public Disposable(Action<float> onLoop, Action<float> action)
            {
                _onLoop = onLoop;
                _action = action;
            }
            public void Dispose()
            {
                _onLoop -= _action;
            }
        }

        private float _value;
        private float _minValue = float.MinValue;
        private float _maxValue = float.MaxValue;

        private float _direction = 0; // 1 for increasing, -1 for decreasing
        private Action<float> _onLoop;

        public float Value
        {
            get => _value;
            set
            {
                float oldValue = _value;
                float rangeSize = _maxValue - _minValue;

                if (rangeSize <= 0f)
                {
                    _value = _minValue;
                    return;
                }

                float relative = (value - _minValue) % rangeSize;
                if (relative < 0f) relative += rangeSize;

                float newValue = _minValue + relative;

                float diff = newValue - oldValue;
                Direction = diff;

                _value = newValue;

                if (Math.Abs(value - oldValue) >= rangeSize)
                {
                    _onLoop?.Invoke(Direction);
                }
            }
        }

        public float Direction
        {
            get => _direction;
            private set => _direction = (value == 0f) ? 0f : value / Math.Abs(value);
        }

        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public LoopableFloat(float initialValue)
        {
            Value = initialValue;
        }

        public LoopableFloat(float minValue, float maxValue, float initialValue = 0f)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            Value = initialValue;
        }

        public LoopableFloat SetMin(float minValue)
        {
            _minValue = minValue;
            if (Value < minValue)
                Value = Value;
            return this;
        }

        public LoopableFloat SetMax(float maxValue)
        {
            _maxValue = maxValue;
            if (Value > maxValue)
                Value = Value;
            return this;
        }
        public IDisposable Subscribe(Action<float> onLoop)
        {
            _onLoop += onLoop;
            return new Disposable(_onLoop, onLoop);
        }
    }
}
