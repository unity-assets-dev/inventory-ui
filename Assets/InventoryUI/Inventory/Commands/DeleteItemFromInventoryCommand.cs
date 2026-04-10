using System.Linq;
using InventoryUI.Inventory;

public class DeleteItemFromInventoryCommand : IExecutionCommand {
    
    private readonly Inventory _inventory;
    private readonly IInventoryLogger _logger;

    public DeleteItemFromInventoryCommand(Inventory inventory, IInventoryLogger logger) {
        _inventory = inventory;
        _logger = logger;
    }
    
    public void Execute() {
        // TODO: Select random slot which not locked and not empty;
        var slot = _inventory.AllOccupiedSlots.Shuffle().FirstOrDefault();
        var slotId = _inventory.GetSlotId(slot);
        
        if (slot != null) {
            // TODO: Clear slot: Удалено ([количество]) [предмет] из слота: [id слота]
            _logger.OnDeleteSlotContent(slot.Item, slot.Count, slotId);
            slot.Clear();
            _inventory.NotifyInventoryUpdated();
            return;
        }
       
        // TODO: if slot not found then: Инвентарь пуст
        _logger.OnInventoryEmpty();
    }
}