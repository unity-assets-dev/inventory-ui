using System;
using InventoryUI.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface ISlotContentListener {
    void SetLockedState(bool isLocked);
    void SetItemsCount(int count);
    void SetEmptyState(bool empty);
    void SetItem(ItemTemplate item);
    void SetStackableState(bool isStackable);
}

public class SlotContentView : InventorySlot, ISlotContentListener {
    
    [SerializeField] private TMP_Text _counterField;
    [SerializeField] private TMP_Text _unlockField;
    [SerializeField] private Image _icon;
    
    private IDisposable _subscription;
    
    public bool IsLocked => Parent.IsLocked;
    public Slot Parent { get; private set; }
    
    public UnityEvent<Slot, Slot> HandleDroppedSlot { get; private set; } = new();
    public UnityEvent<SlotContentView> SlotClicked { get; private set; } = new();
    
    public void AssignSlot(Slot slot) {
        Parent = slot;
        _unlockField.text = slot.UnlockPrice.ToString();
        _subscription = slot.AddListener(this);
        name = $"[Slot: {slot.Index}]";
    }

    public void Dispose() {
        _subscription?.Dispose();
        //Destroy(gameObject);
    }
    
    public void SetStackableState(bool stackable) => _counterField.gameObject.SetActive(stackable);
    public void SetItemsCount(int count) {
        _counterField.text = count.ToString();
        SetEmptyState(count >= 1);
        SetStackableState(Parent.IsStackable);
    }

    public void SetItem(ItemTemplate item) {
        _icon.sprite = item != null ? item.ItemIcon : null;
        _icon.gameObject.SetActive(_icon.sprite != null);
    }

    public void OnDrop(Slot slot) {
        if (!slot.IsLocked && Parent.IsLocked) return;
        
        Parent.SwapWith(slot);
        // Or
        // HandleDroppedSlot?.Invoke(Parent, slot); // with external controller;
    }

    public void HandleClick() => SlotClicked?.Invoke(this);
    
}