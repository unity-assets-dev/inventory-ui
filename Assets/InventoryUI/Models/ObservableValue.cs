using System;
using System.Collections.Generic;

namespace InventoryUI.Inventory {
    
    public interface IObservableValue<T> {
        T Value { get; set; }
        
        IDisposable AddListener(Action<T> onSubscribe);
        void RemoveListener(Action<T> onUnsubscribe);
    }
    
    public class ObservableValue<T> : IObservableValue<T> {
        
        private event Action<T> _onChange;
        
        private readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;
        private T _value;
        private readonly bool _notifyOnChange;

        public T Value {
            get => _value;
            set {
                if (_comparer.Equals(_value, value)) return;
                
                _value = value;
                
                if(_notifyOnChange)
                    _onChange?.Invoke(value);
            }
        }

        public ObservableValue(T defaults, bool notifyOnChange = true) {
            _value = defaults;
            _notifyOnChange = notifyOnChange;
        }
        
        public IDisposable AddListener(Action<T> onSubscribe) {
            _onChange += onSubscribe;
            onSubscribe?.Invoke(_value);
            return new SlotSubscriptionHandler(() => _onChange -= onSubscribe);
        }

        public void RemoveListener(Action<T> onUnsubscribe) => _onChange -= onUnsubscribe;
    }
}