using Game;
using UnityEngine;
using UnityEngine.Audio;

namespace Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] InventoryItem item;
        [SerializeField] ItemData itemData;
        [SerializeField] AudioClip audioClip;
        [SerializeField] AudioMixerGroup audioMixer;

        internal ItemData GetItemData() => itemData;

        private void OnEnable()
        {
            GameController.I.RegisterItem(itemData, gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                PickUp();
        }

        internal void Load(ItemData item)
        {
            if (item != null)
                itemData = item;
            Setup(itemData.used);
        }

        private void Setup(bool used)
        {
            itemData.used = used;
            gameObject.SetActive(!used);
        }

        private void PickUp()
        {
            bool wasPickedUp = InventoryController.I.AddItem(item);
            if (wasPickedUp)
            {
                Tools.SimpleAudio.PlayClipAtPoint(audioClip, transform.position, 1, audioMixer);
                Setup(true);
            }
        }

        [ContextMenu("GenerateId")]
        private void GenerateId()
        {
            itemData.id = System.Guid.NewGuid().ToString();
            itemData.type = item.type;
        }
    }
}
