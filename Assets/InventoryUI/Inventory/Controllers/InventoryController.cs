namespace InventoryUI.Inventory {
    public class InventoryController: IExpandable {
        
        private readonly Inventory _inventory;
        private readonly InventoryScreen _screen;
        private readonly InventoryModel _model;

        public InventoryController(InventoryScreen screen, InventoryModel model, Inventory inventory) {
            _inventory = inventory;
            _screen = screen;
            _model = model;
        }

        public void Show() {
            if (_screen.TryGetElement<InventoryExpansion>(out var extension)) {
                extension.AssignSlots(_inventory);
                
                _inventory.InventoryWeightChanged.AddListener(OnInventoryWeightChanged);
                extension.SlotWantToUnlock.AddListener(OnInventorySlotWantToUnlock);
            }
        }

        private void OnInventorySlotWantToUnlock(Slot slot) {
            if (slot.UnlockPrice > _model.Coins.Value || !_inventory.SlotMayUnlocked(slot)) return;

            _model.Coins.Value -= slot.UnlockPrice;
            slot.Unlock();
        }

        private void OnInventoryWeightChanged(float weight) => _model.Weight.Value = weight;

        public void Hide() {
            if (_screen.TryGetElement<InventoryExpansion>(out var extension)) {
                _inventory.InventoryWeightChanged.RemoveListener(OnInventoryWeightChanged);
                extension.SlotWantToUnlock.RemoveListener(OnInventorySlotWantToUnlock);
                extension.Dispose();
            }
        }
    }
}