using UnityEngine;

public class StatsExpansion : MonoBehaviour, ILayoutElement {
    
    [SerializeField] private StatField _coins;
    [SerializeField] private StatField _weight;
    
    public void UpdateCoins(int value) => _coins.UpdateField(value.ToString());
    
    public void UpdateWeight(float value) => _weight.UpdateField(value.ToString("F3"));
}