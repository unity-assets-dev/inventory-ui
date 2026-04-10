using InventoryUI.Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Templates/Create TorsoEquipmentTemplate", fileName = "TorsoEquipmentTemplate", order = 0)]
public class TorsoEquipmentTemplate : EquipmentTemplate {
    protected override ItemType GetItemType() => ItemType.Torso;
}