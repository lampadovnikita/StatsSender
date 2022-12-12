using System;

namespace Assets.CharacterStatsSemder.Scripts.Observable
{ 
    public class ObservableValue<T> where T : IEquatable<T>
    {
        public event Action<T, T> ValueChanged;

        private T value;

        public T Value 
        {
            get
            {
                return value;
            }
            set
            {            
                if (value.Equals(this.value) == false)
                {
                    T prevValue = this.value;

                    this.value = value;

                    ValueChanged?.Invoke(prevValue, value);
                }
            }
        }
    }
}