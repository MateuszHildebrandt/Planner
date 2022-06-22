using System;
using UnityEngine;

namespace Inventory
{
    public enum InventoryType { Health, Magic };

    [CreateAssetMenu(fileName = "New item", menuName = "Scriptable Object/Inventory/Item")]
    public class InventoryItem : ScriptableObject
    {
        public new string name = string.Empty;
        public InventoryType type;
        public bool used;
        public int quantity = 0;
        public int quantityMax = 0;
    }
}
