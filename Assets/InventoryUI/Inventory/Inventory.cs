using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace InventoryUI.Inventory {
    public class Inventory: IEnumerable<Slot> {
        
        private readonly HashSet<Slot> _slots;

        public Inventory(int count) {
            _slots = new HashSet<Slot>(count);
            for (var i = 0; i < count; i++) {
                var slot = new Slot(i);
                _slots.Add(slot);
                
                if(i < 15)
                    slot.Unlock();
            }
        }
        
        public IEnumerable<Slot> NonLockedSlots => _slots.Where(s => !s.IsLocked);
        
        public IEnumerable<Slot> AllOccupiedSlots => NonLockedSlots.Where(s => !s.IsEmpty);
        public IEnumerable<Slot> Free => NonLockedSlots.Where(s => s.IsEmpty);
        
        public int AvailableSlots => Free.Count(); // resolve enumerable;

        public IObservableValue<float> InventoryWeightChanged { get; } = new ObservableValue<float>(0);
        
        public bool AddNonStackableItem(ItemTemplate template, out int slotId) {
            slotId = -1;
            
            if(AvailableSlots <= 0) return false; // no available slots to add new item;
                
            var freeRandomSlot = Free.Shuffle().FirstOrDefault();
                
            slotId = _slots.ToList().IndexOf(freeRandomSlot);
                
            freeRandomSlot?.AssignItem(template);

            return true;
        }

        public bool AddStackableItem(ItemTemplate template, int count, out int slotId) {
            slotId = -1;
            
            if(AvailableSlots <= 0) return false; // no available slots to add new item;
                
            var freeRandomSlot = Free.Shuffle().FirstOrDefault();
                
            slotId = _slots.ToList().IndexOf(freeRandomSlot);
                
            freeRandomSlot?.AssignItem(template, count <= 0? 1: count);

            return true;
        }

        public bool SpreadStackableItem(ItemTemplate template, int count, out int slotId) {
            slotId = -1;
            
            var slots = new Queue<Slot>(AllOccupiedSlots.Where(s => s.Item.ItemId == template.ItemId && s.HasSpace));

            var counts = count;

            while (slots.Count > 0) {
                var slot = slots.Dequeue();
                slotId = _slots.ToList().IndexOf(slot);
                counts -= slot.AddTo(counts);
            }
            
            if(counts <= 0) return true;
            
            if(AvailableSlots <= 0) return false;
            
            var newStacks = counts / template.MaxStackSize + 1;

            for (var stack = 0; stack < newStacks; stack++) {
                if (counts == 0) continue;
                
                var stackSize = Mathf.Min(template.MaxStackSize, counts);
                AddStackableItem(template, counts, out slotId);
                counts -= stackSize;
            }

            return true;
        }
        
        public bool AddItem(ItemTemplate template, int count, out int slotId) {
            slotId = -1;
            
            return !template.Stackable ? 
                AddNonStackableItem(template, out slotId) : 
                SpreadStackableItem(template, count, out slotId);
        }
        
        public int GetSlotId(Slot slot) => _slots.ToList().IndexOf(slot);

        public Slot[] GetSlotsByType(ItemType itemType) => AllOccupiedSlots.Where(s => s.Item.ItemType == itemType).ToArray();

        public Slot[] GetSlotsByItemId(string itemId) => AllOccupiedSlots.Where(s => s.Item.ItemId == itemId).ToArray();

        public void SwapSlots(Slot source, Slot target) {
            if(!source.CanSwapTo(target)) return;
            
            source.SwapWith(target);
        }
        
        public IEnumerator<Slot> GetEnumerator() => _slots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void NotifyInventoryUpdated() {
            var weightTotal = AllOccupiedSlots
                .Where(s => s.Count > 0)
                .Sum(s => s.Count * s.Item.Weight);
            
            InventoryWeightChanged.Value = weightTotal;
        }

        public bool SlotMayUnlocked(Slot slot) {
            var buffer = _slots.ToList();
            
            for (var i = slot.Index - 1; i >= 0; i--)
                if(buffer[i].IsLocked) return false;
            
            return true;
        }

        public void Replace(Slot[] slots) {
            var buffer = _slots.ToList();
            
            for (var index = 0; index < slots.Length; index++) {
                var source = slots[index];
                var target = buffer[index];
                
                target.Clear();
                target.AssignItem(source.Item, source.Count);

                if (!source.IsLocked) target.Unlock();
            }
            
            NotifyInventoryUpdated();
        }
    }
}