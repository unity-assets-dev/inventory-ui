using InventoryUI.Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Templates/Create AmmoTemplate", fileName = "AmmoTemplate", order = 0)]
public class AmmoTemplate : ItemTemplate {
    protected override ItemType GetItemType() => ItemType.Ammo;

    public override void DisplayOver(IDetailsViewer viewer, int parentCount) {
        base.DisplayOver(viewer, parentCount);
        
        viewer.SetContent("");
    }
}