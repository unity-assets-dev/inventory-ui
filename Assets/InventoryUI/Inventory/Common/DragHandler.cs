using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler {
    
    [SerializeField] private SlotContentView _slot;
    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _group;

    private Vector2 _dragOffset;

    public SlotContentView Slot => _slot;

    private void OnValidate() {
        _canvas ??= GetComponent<Canvas>();
        _group ??= GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (Slot.IsLocked) return;
        
        _group.blocksRaycasts = false;
        _canvas.overrideSorting = true;
        _canvas.sortingOrder = 100;
        _dragOffset = (Vector2)transform.position - eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (Slot.IsLocked) return;
        
        _group.blocksRaycasts = true;
        _canvas.sortingOrder = 1;
        _canvas.overrideSorting = false;
        
        transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData) {
        if (Slot.IsLocked) return;
        
        transform.position = eventData.position + _dragOffset;
    }

    public void OnDrop(PointerEventData eventData) {
        if (!eventData.pointerDrag.TryGetComponent<DragHandler>(out var handler) || handler == this) return;
        Debug.Log($"Drag`n`drop from {handler.Slot.name} to {Slot.name}");
        Slot.OnDrop(handler.Slot.Parent);
    }

    public void OnPointerClick(PointerEventData eventData) => Slot.HandleClick();
}