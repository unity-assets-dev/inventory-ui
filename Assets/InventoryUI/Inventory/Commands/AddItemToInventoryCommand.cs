using System.Linq;
using InventoryUI.Inventory;

public class AddItemToInventoryCommand : IExecutionCommand {
    
    private readonly TemplateFactory _templates;
    private readonly Inventory _inventory;
    private readonly IInventoryLogger _logger;

    public AddItemToInventoryCommand(TemplateFactory templates, Inventory inventory, IInventoryLogger logger) {
        _templates = templates;
        _inventory = inventory;
        _logger = logger;
    }
    public void Execute() {
        // TODO: Randomize type from : Head, Torso, Weapon;
        var type = _templates.Get(new[] {
            ItemType.Torso, 
            ItemType.Head, 
            ItemType.Weapon
        }).Shuffle().FirstOrDefault();
        
        // TODO: Add one item of type previously selected;
        if (_inventory.AddNonStackableItem(type, out var slotId)) {
            // TODO: if item successfully add: log: Добавлено [предмет] в слот: [id слота]
            _logger.OnItemAdded(type, slotId);
            _inventory.NotifyInventoryUpdated();
        }
        else {
            // TODO: else : Инвентарь полон
            _logger.OnInventoryFull();
        }
    }
}