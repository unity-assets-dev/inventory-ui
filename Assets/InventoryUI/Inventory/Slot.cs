using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI.Inventory {

    public interface ISlot {
        
    }
    
    public class Slot : ISlot {
        
        private readonly HashSet<ISlotContentListener> _listeners = new();
        private int _count;
        private ItemTemplate _item;

        public bool IsLocked { get; private set; } = true;
        public bool IsEmpty => Item == null || _count == 0;
        public bool IsStackable => Item != null && Item.Stackable;
        public bool HasSpace => Item == null || _count < Item.MaxStackSize;
        
        public int Index { get; }

        public Slot(int index) => Index = index;

        public int Count {
            get => _count;
            set {
                _count = Mathf.Min(value, Item != null? Item.MaxStackSize: 0);
                
                if(_count > 0 && IsStackable)
                    foreach (var listener in _listeners) listener.SetItemsCount(_count);
                
                if(_count <= 0)
                    Clear();
            }
        }

        public ItemTemplate Item {
            get => _item;
            set {
                if (value != _item) {
                    _item = value;
                    _count = _item == null? 0: 1;

                    UpdateNotificationToSubscriptions();
                }
            }
        }

        public int UnlockPrice => Index < 15 ? 0 : Index / 5 * 20;

        public IDisposable AddListener(ISlotContentListener contentListener) {
            _listeners.Add(contentListener);
            
            NotifyContentChanges(contentListener);
            
            return new SlotSubscriptionHandler(() => _listeners.Remove(contentListener));
        }
        
        public void Unlock() {
            IsLocked = false;

            UpdateNotificationToSubscriptions();
        }

        public void Clear() {
            Item = null;

            UpdateNotificationToSubscriptions();
        }

        public void AssignItem(ItemTemplate template, int count = 1) {
            Item = template;
            Count = count;
        }

        public int AddTo(int count) {
            var before = _count;
            Count = Mathf.Min(_count + count, Item.MaxStackSize);
            return Count - before;
        }
        
        public void SwapTo(Slot targetSlot) {
            var sourceItem = Item;
            var sourceCount = Count;
            
            var targetItem = targetSlot.Item;
            var targetCount = targetSlot.Count;
            
            AssignItem(targetItem, targetCount);
            targetSlot.AssignItem(sourceItem, sourceCount);
        }

        public void FillFor(Slot targetSlot) {
            var sum = targetSlot.Count + Count;
            if (sum <= Item.MaxStackSize) {
                targetSlot.AddTo(Count);
                Clear();
                return;
            }
            
            var restAmount = Item.MaxStackSize - sum;
            targetSlot.AddTo(Item.MaxStackSize);
            Count = restAmount;
        }
        
        public bool CanSwapTo(Slot target) {
            if(this == target) return false; // same slot;
            
            if(IsLocked || target.IsLocked) return false; // one of slot is locked;
            
            if(Item == null && target.Item == null )  return false; // both slots are empty - just skip swap operation;
            
            return true;
        }

        public void SwapWith(Slot target) {
            if(IsEmpty && target.IsEmpty) return;
            
            if (Item != target.Item || !HasSpace || !target.HasSpace) {
                // TODO: Exchange slot contents;
                SwapTo(target);
                return;
            }
            // TODO: Just fill same content amount to another slot; 
            FillFor(target);
        }

        private void UpdateNotificationToSubscriptions() {
            foreach (var contentListener in _listeners) NotifyContentChanges(contentListener);
        }

        private void NotifyContentChanges(ISlotContentListener listener) {
            if (IsStackable)
                listener.SetItemsCount(_count);

            listener.SetLockedState(IsLocked);
            listener.SetStackableState(IsStackable);
            listener.SetEmptyState(IsEmpty);
            listener.SetItem(_item);
        }

        
    }

    internal class SlotSubscriptionHandler : IDisposable {
        private readonly Action _onDispose;

        public SlotSubscriptionHandler(Action onDispose) => _onDispose = onDispose;

        public void Dispose() => _onDispose?.Invoke();
    }
    
}