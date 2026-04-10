using UnityEngine;

namespace InventoryUI.Inventory {
    
    public interface IInventoryLogger {
        void OnCoinsAdded(int coins);
        void OnInventoryEmpty();
        void OnInventoryFull();
        void OnItemAdded(ItemTemplate type, int slotId);
        void OnDeleteSlotContent(ItemTemplate slotItem, int itemsCount, int slotId);
        void OnInventoryMissingWeapon();
        void OnInventoryMissingWeaponAmmo(string weaponId);
        void OnUsingWeaponAmmo(string weaponId, string ammoId, float weaponDamage);
    }
    
    public class InventoryLogger : IInventoryLogger {
        public void OnCoinsAdded(int coins) => Debug.Log($"Добавлено ({coins}) монет");

        public void OnInventoryEmpty() => Debug.Log("Инвентарь пуст");

        public void OnInventoryFull() => Debug.Log("Инвентарь полон");

        public void OnItemAdded(ItemTemplate type, int slotId) => Debug.Log($"Добавлено [{type.ItemId}] в слот: [{slotId}]");

        public void OnDeleteSlotContent(ItemTemplate slotItem, int itemsCount, int slotId) => Debug.Log($"({itemsCount}) [{slotItem.ItemId}] из слота: [{slotId}");
        
        public void OnInventoryMissingWeapon() => Debug.Log("Нет оружия");
        
        public void OnInventoryMissingWeaponAmmo(string weaponId) => Debug.Log($"Нет патронов для [{weaponId}]");
        
        public void OnUsingWeaponAmmo(string weaponId, string ammoId, float weaponDamage) => Debug.Log($"Выстрел из [{weaponId}], патроны: [{ammoId}], урон: [{weaponDamage}]");
    }
}