using InventoryUI.Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Templates/Create HeadEquipmentTemplate", fileName = "HeadEquipmentTemplate", order = 0)]
public class HeadEquipmentTemplate : EquipmentTemplate {
    protected override ItemType GetItemType() => ItemType.Head;
}