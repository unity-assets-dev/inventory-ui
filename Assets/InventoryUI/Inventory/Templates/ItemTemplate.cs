using InventoryUI.Inventory;
using UnityEngine;

public abstract class ItemTemplate : ScriptableObject {
    
    [SerializeField] private string _itemId;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private float _weight;
    [SerializeField] private int _maxStackSize = 1;
    
    public string ItemId => _itemId;
    public Sprite ItemIcon => _itemIcon;
    public float Weight => _weight;
    public int MaxStackSize => _maxStackSize;
    public bool Stackable => _maxStackSize > 1;
    
    public ItemType ItemType => GetItemType();
    protected abstract ItemType GetItemType();

    public virtual void DisplayOver(IDetailsViewer viewer, int count) {
        viewer.SetIcon(ItemIcon);
        viewer.SetId(ItemId);
        viewer.SetType(ItemType, Weight * count);
        
    }
}