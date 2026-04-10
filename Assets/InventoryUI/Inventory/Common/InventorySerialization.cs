using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventoryUI.Inventory {
    
    public class InventorySerialization {
        
        private readonly InventoryModel _model;
        private readonly Inventory _inventory;
        private readonly ISerializationContainer _container;

        public InventorySerialization(InventoryModel model, Inventory inventory, ISerializationContainer container) {
            _model = model;
            _inventory = inventory;
            _container = container;
        }
        
        public void Serialize() {
            var result = new List<SerializedSlot>();
            
            foreach (var slot in _inventory) {
                result.Add(new SerializedSlot() {
                    itemId = slot.Item == null? "" : slot.Item.ItemId,
                    count = slot.Count,
                    locked = slot.IsLocked,
                });
            }
            
            _container.Save(new SerializedInventory() {
                coins = _model.Coins.Value,
                slots = result.ToArray(),
            }, "stashed_inventory");
        }

        public void Deserialize(TemplateFactory factory) {
            var serializedInventory = new SerializedInventory();
            var defaults = JsonUtility.ToJson(serializedInventory);
            
            var inventory = _container.LoadSaved<SerializedInventory>("stashed_inventory", defaults);
            
            _model.Coins.Value = inventory.coins;
            var slots = inventory.Deserialize(factory);
            _inventory.Replace(slots);
        }
    }

    public class SerializedInventory {
        public int coins;
        public SerializedSlot[] slots;

        public SerializedInventory() {
            slots = Enumerable.Range(0, 50).Select(i => new SerializedSlot() {
                itemId = "",
                index = i,
                count = 0,
                locked = i >= 15,
            }).ToArray();
        }

        public Slot[] Deserialize(TemplateFactory factory) {
            var result = new List<Slot>();
            
            foreach (var slot in slots) {
                var item = new Slot(slot.index);

                if (!string.IsNullOrEmpty(slot.itemId)) {
                    var template = factory.Get(slot.itemId);
                    if(template) // only if template was found
                        item.AssignItem(template, slot.count);
                }
                
                if(!slot.locked)
                    item.Unlock();
                
                result.Add(item);
            }
            
            return result.ToArray();
        }
    }
    
    [Serializable]
    public class SerializedSlot {
        public string itemId;
        public int index;
        public int count;
        public bool locked;
    }
}