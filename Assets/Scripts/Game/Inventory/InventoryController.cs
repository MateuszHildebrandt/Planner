using Player;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] PlayerData playerData;

        public static InventoryController I { get; private set; }

        private void Awake()
        {
            I = this;
        }

        public bool AddItem(InventoryItem item)
        {
            if (item.type == InventoryType.Health)
                playerData.ClampHealth(playerData.health + item.quantity);
            else if (item.type == InventoryType.Magic)
                playerData.ClampMagic(playerData.magic + item.quantity);

            return true;
        }
    }
}
