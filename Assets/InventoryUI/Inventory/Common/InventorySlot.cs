using UnityEngine;

public abstract class InventorySlot : MonoBehaviour {
    
    [SerializeField] private GameObject _locked;
    [SerializeField] private GameObject _unlocked;
    [SerializeField] private GameObject _content;

    public void SetLockedState(bool locked) {
        _locked.SetActive(locked);
        _unlocked.SetActive(!locked);
    }

    public void SetEmptyState(bool empty) {
        //_content.SetActive(!empty);
    }
}