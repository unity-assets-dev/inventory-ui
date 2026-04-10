using System;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class InventoryAction : MonoBehaviour, IActionElement {
    
    [SerializeField, HideInInspector] private Button _button;

    [SerializeField, OnValueChanged(nameof(OnCaptionChanged))]
    private string _caption;

    [SerializeField] private TMP_Text _captionField;
    
    [SerializeField, OnValueChanged(nameof(OnIconChanged))]
    private Sprite _iconSprite;

    [SerializeField] private Image _icon;
    private Action _onClick;

    private void OnValidate() {
        _button ??= GetComponent<Button>();
        _captionField ??= GetComponentInChildren<TMP_Text>();
        _icon ??= GetComponentsInChildren<Image>().FirstOrDefault(s => s.transform != transform);
        name = $"[{GetType().Name}]";
    }

    private void OnCaptionChanged() => _captionField.text = _caption;

    private void OnIconChanged() => _icon.sprite = _iconSprite;

    private void OnClick() => _onClick?.Invoke();
    
    public void AddListener(Action onClick) {
        _onClick = onClick;
        _button.onClick.AddListener(OnClick);
    }

    public void RemoveListener(Action onClick) => _button.onClick.RemoveListener(OnClick);

    public void Dispose() => _button.onClick.RemoveAllListeners();
}