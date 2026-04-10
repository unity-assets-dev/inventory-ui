using System.Text;
using InventoryUI.Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Templates/Create WeaponTemplate", fileName = "WeaponTemplate", order = 0)]
public class WeaponTemplate : ItemTemplate {
    
    [SerializeField] private AmmoTemplate _ammo;
    [SerializeField] private float _damage;
    
    public AmmoTemplate Ammo => _ammo;
    public float Damage => _damage;
    protected override ItemType GetItemType() => ItemType.Weapon;

    public override void DisplayOver(IDetailsViewer viewer, int parentCount) {
        base.DisplayOver(viewer, parentCount);
        
        var content = new StringBuilder();
        
        content.AppendLine($"[Ammo] \t {Ammo.ItemId}");
        content.AppendLine($"[Damage] \t {Damage}");
        
        viewer.SetContent(content.ToString());
    }
}