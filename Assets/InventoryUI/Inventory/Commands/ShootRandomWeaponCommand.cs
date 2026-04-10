using System.Linq;
using InventoryUI.Inventory;

public class ShootRandomWeaponCommand : IExecutionCommand {
    
    private readonly Inventory _inventory;
    private readonly IInventoryLogger _logger;

    public ShootRandomWeaponCommand(Inventory inventory, IInventoryLogger logger) {
        _inventory = inventory;
        _logger = logger;
    }
    
    public void Execute() {
        // TODO: Select random weapon type
        var weaponSlot = _inventory
            .GetSlotsByType(ItemType.Weapon)
            .Shuffle().FirstOrDefault();

        if (weaponSlot == null || weaponSlot.Item is not WeaponTemplate weapon) {
            // TODO: if inventory doesn't contain weapon: Нет оружия {of type}
            _logger.OnInventoryMissingWeapon();
            return;
        }

        // TODO: Select ammo for these weapon
        var ammoSlot = _inventory
            .GetSlotsByItemId(weapon.Ammo.ItemId)
            .Shuffle().FirstOrDefault();

        if (ammoSlot == null || ammoSlot.Item is not AmmoTemplate ammo) {
            // TODO: if has no ammo for these weapon then: Нет патронов для [оружие]
            _logger.OnInventoryMissingWeaponAmmo(weapon.ItemId);
            return;
        }

        ammoSlot.Count--;
        
        // TODO: if weapon and ammo are selected, then spend ammo with debug: Выстрел из [оружие], патроны: [тип патронов], урон: [урон]
        _logger.OnUsingWeaponAmmo(weapon.ItemId, ammo.ItemId, weapon.Damage);
        _inventory.NotifyInventoryUpdated();
    }
}