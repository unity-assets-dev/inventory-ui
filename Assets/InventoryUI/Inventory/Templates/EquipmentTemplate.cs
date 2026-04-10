using System.Text;
using UnityEngine;

public abstract class EquipmentTemplate : ItemTemplate {
    [SerializeField] private float _protection;
    
    public float Protection => _protection;

    public override void DisplayOver(IDetailsViewer viewer, int parentCount) {
        base.DisplayOver(viewer, parentCount);
        
        var content = new StringBuilder();
        
        content.AppendLine($"[Protection] \t {Protection}");
        
        viewer.SetContent(content.ToString());
    }
}