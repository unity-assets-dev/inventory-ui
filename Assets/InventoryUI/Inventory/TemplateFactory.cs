using System;
using System.Linq;
using UnityEngine;

namespace InventoryUI.Inventory {
    
    [CreateAssetMenu(menuName = "Create TemplateFactory", fileName = "TemplateFactory", order = 0)]
    public class TemplateFactory : ScriptableObject {
        
        [SerializeField] public ItemTemplate[] _templates;

        private bool TryGetTemplate(string id, out ItemTemplate template) {
            template = _templates.FirstOrDefault(t => t.ItemId == id);
            
            return template != null;
        }
        
        public ItemTemplate Get(string itemName) {
            if (TryGetTemplate(itemName, out var template)) {
                return template;
            }
            
            throw new NullReferenceException("Template not found");
        }

        public ItemTemplate[] Get(ItemType[] types) => _templates.Where(t => types.Contains(t.ItemType)).ToArray();
    }
}