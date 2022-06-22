using Newtonsoft.Json;
using Tools;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Scriptable Object/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public Float2 position;
        public float health;
        public float maxHealth;
        public float magic;
        public float maxMagic;

        public void ClampHealth(float value) => health = Mathf.Clamp(value, 0, maxHealth);
        public void ClampMagic(float value) => magic = Mathf.Clamp(value, 0, maxMagic);

        public void ResetData()
        {
            position = new Float2(10, 10);
            health = 50;
            maxHealth = 100;
            magic = 30;
            maxMagic = 100;
        }
    }
}
