using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [Header("Resoureces")]
        [SerializeField] GameData gameData;
        [SerializeField] Player.PlayerController playerController;

        private static GameController _instance;
        public static GameController I
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameController>();
                return _instance;
            }
        }

        private Dictionary<string, GameObject> _itemsPair = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _mobsPair = new Dictionary<string, GameObject>();

        private void Start()
        {
            Load();
        }

        private void OnApplicationQuit()
        {
            if(Application.isEditor == false)
                Save();

            gameData.items = null;
            gameData.mobs = null;
        }

        internal void RegisterItem(ItemData itemData, GameObject go)
        {
            if (string.IsNullOrEmpty(itemData.id))
            {
                Debug.LogWarning($"Item {go.name} ID is empty.", go);
                return;
            }

            if (gameData.items.ContainsKey(itemData.id) == false)
            {
                gameData.items.Add(itemData.id, itemData);
                Debug.Log($"Register item: {itemData.id}", go);
            }

            if (_itemsPair.ContainsKey(itemData.id) == false)
                _itemsPair.Add(itemData.id, go);
        }

        internal void RegisterMob(MobData mobData, GameObject go)
        {
            if (string.IsNullOrEmpty(mobData.id))
            {
                Debug.LogWarning($"Mob {go.name} ID is empty.", go);
                return;
            }

            if (gameData.mobs.ContainsKey(mobData.id) == false)
            {
                gameData.mobs.Add(mobData.id, mobData);
                Debug.Log($"Register mob: {mobData.id}", go);
            }

            if (_mobsPair.ContainsKey(mobData.id) == false)
                _mobsPair.Add(mobData.id, go);
        }

        #region LoadSave
        [ContextMenu("Load")]
        internal void Load()
        {
            bool loaded = FilesManager.LoadScriptableObject(FilesManager.PlayerDataPath, gameData, false);

            if (loaded)
            {
                foreach (var mapItem in gameData.items)
                    _itemsPair[mapItem.Key].GetComponent<Inventory.ItemPickup>().Load(mapItem.Value);

                foreach (var mapMob in gameData.mobs)
                    _mobsPair[mapMob.Key].GetComponent<Mob.MobController>().Load(mapMob.Value);
            }
            else
                gameData.playerData.ResetData();

            playerController.Load();
        }

        [ContextMenu("Save")]
        internal void Save()
        {
            if (gameData != null)
                FilesManager.Save(gameData, FilesManager.PlayerDataPath, false);
        }
        #endregion
    }
}
