using System.Linq;
using InventoryUI.Inventory;
using UnityEngine;

public class AddAmmoToInventoryCommand : IExecutionCommand {
    
    private readonly TemplateFactory _templates;
    private readonly Inventory _inventory;
    private readonly IInventoryLogger _logger;

    public AddAmmoToInventoryCommand(TemplateFactory templates, Inventory inventory, IInventoryLogger logger) {
        _templates = templates;
        _inventory = inventory;
        _logger = logger;
    }
    public void Execute() {
        var type = _templates.Get(new[] {
            ItemType.Ammo
        }).Shuffle().FirstOrDefault();
        
        // TODO: randomize ammo type and amount of these: 10..30
        var count = Random.Range(10, 31);
        
        // TODO: Add one item of type previously selected;
        // TODO: Add ammo to inventory fill slots with same type before create new
        if (_inventory.SpreadStackableItem(type, count, out var slotId)) {
            // TODO: if success: Добавлено ([количество]) [тип патронов] в слот: [id слота]
            _logger.OnItemAdded(type, slotId);
            _inventory.NotifyInventoryUpdated();
        }
        else {
            // TODO: else : Инвентарь полон
            _logger.OnInventoryFull();
        }
    }
}