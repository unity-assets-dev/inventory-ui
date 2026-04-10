using System;
using System.Collections.Generic;
using InventoryUI.Inventory;
using UnityEngine;
using UnityEngine.Events;

public class InventoryExpansion : MonoBehaviour, ILayoutElement {
    [SerializeField] private ItemDetailsView _detailsViewer;
    [SerializeField] private SlotContentView _prefab;
    
    private readonly List<SlotContentView> _slots = new();
    
    public UnityEvent<Slot> SlotWantToUnlock { get; } = new();
    
    private void Awake() {
        _detailsViewer.HideOut();
    }

    private SlotContentView Create(Slot slot) {
        var instance = Instantiate(_prefab, _prefab.transform.parent);
        
        instance.gameObject.SetActive(true);
        instance.AssignSlot(slot);
        instance.SlotClicked.AddListener(OnSlotClicked);
        // TODO: Subscribe for slot view actions;
        
        return instance;
    }

    private void OnSlotClicked(SlotContentView slot) {
        if (slot.IsLocked) {
            // TODO: Request to unlock;
            SlotWantToUnlock?.Invoke(slot.Parent);
            return;
        }
        
        _detailsViewer.DisplayAbout(slot);
    }

    private void Update() {
        if (_detailsViewer.IsActive && Input.GetMouseButtonDown(0)) {
            _detailsViewer.Hide();
        }
    }

    public void AssignSlots(IEnumerable<Slot> inventory) {
        foreach (var slot in inventory) _slots.Add(Create(slot));
    }

    public void Dispose() {
        foreach (var slot in _slots.ToArray()) {
            slot.Dispose();
            slot.SlotClicked.RemoveListener(OnSlotClicked);
            _slots.Remove(slot);
        }
    }
}
