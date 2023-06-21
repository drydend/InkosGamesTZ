using System;

namespace Utils
{
    public class Counter
    {
        public int Value { get; private set; }

        public event Action OnValueChanged;
        public void Increase(int value = 1)
        {
            Value += value;
            OnValueChanged?.Invoke();
        }

        public void Reset()
        {
            Value = 0;
            OnValueChanged?.Invoke();
        }
    }
}