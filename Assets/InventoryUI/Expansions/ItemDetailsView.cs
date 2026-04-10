using DG.Tweening;
using InventoryUI.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDetailsViewer {
    void SetIcon(Sprite itemIcon);
    void SetId(string itemId);
    void SetContent(string content);
    void SetType(ItemType itemType, float weight);
}

public class ItemDetailsView : MonoBehaviour, IDetailsViewer {
    
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _id;
    [SerializeField] private TMP_Text _type;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private TMP_Text _params;
    [SerializeField] private RectTransform _content;
    
    [SerializeField] private CanvasGroup _group;
    public bool IsActive { get; private set; }

    public void DisplayAbout(SlotContentView slot) {
        if(slot.IsLocked || slot.Parent.IsEmpty) return;

        var item = slot.Parent.Item;
        
        _count.text = item.Stackable? 
            $"{slot.Parent.Count}/{item.MaxStackSize}":
            $"{slot.Parent.Count}";
        
        item.DisplayOver(this, slot.Parent.Count);
        Show();
    }

    public void Hide() {
        DOTween
            .Sequence()
            .Append(_group.DOFade(0, 0.1f))
            .OnComplete(() => {
                IsActive = false;
                _group.blocksRaycasts = IsActive;
            })
            .Play();
    }

    private void Show() {
        gameObject.SetActive(true);
        
        DOTween
            .Sequence()
            .Append(_group.DOFade(1, 0.1f))
            .OnComplete(() => {
                IsActive = true;
                _group.blocksRaycasts = IsActive;
            })
            .Play();
    }

    public void SetIcon(Sprite itemIcon) => _icon.sprite = itemIcon;

    public void SetId(string itemId) => _id.text = itemId;

    public void SetContent(string content) => _params.text = content;
    public void SetType(ItemType itemType, float weight) => _type.text = $"[Type] <b>{itemType}</b>\n[Weight] {weight} kg.";

    public void HideOut() {
        _group.alpha = 0;
        _group.blocksRaycasts = false;
        
        gameObject.SetActive(false);
    }
}