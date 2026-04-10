using TMPro;
using UnityEngine;

public class StatField : MonoBehaviour {
    
    [SerializeField] private TMP_Text _statField;
    [SerializeField] private string _prefix;

    private void OnValidate() {
        name = $"[StatField]";
    }

    public void UpdateField(string value) => _statField.text = $"{_prefix}: {value}";
}