using Inventory;
using Player;
using System;
using Tools;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Scriptable Object/GameData")]
    public class GameData : ScriptableObject
    {
        public PlayerData playerData;

        public UDictionary<string, ItemData> items = new UDictionary<string, ItemData>();
        public UDictionary<string, MobData> mobs = new UDictionary<string, MobData>();
    }

    [Serializable]
    public class ItemData
    {
        public string id;
        public InventoryType type;
        public bool used = false;
    }

    [Serializable]
    public class MobData
    {
        public string id;
        public bool live = true;
        public Float2 position;
    }
}
