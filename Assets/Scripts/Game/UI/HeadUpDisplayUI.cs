using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HeadUpDisplayUI : MonoUI
    {
        [Header("References")]
        [SerializeField] Image healthValue;
        [SerializeField] Image magicValue;
        [Header("Resources")]
        [SerializeField] PlayerData playerData;

        private RemapImageColor _remapHealthColor;
        private float _timer;

        private void Start()
        {
            _remapHealthColor = healthValue.GetComponent<RemapImageColor>();
        }

        private void Update()
        {
            if (IsActive() == false)
                return;

            _timer += Time.deltaTime;

            if (_timer > 0.2f)
            {
                UpdateHealth();
                UpdateMagic();
                _timer = 0;
            }
        }

        internal void UpdateHealth()
        {
            float normalizeValue = playerData.health / playerData.maxHealth;
            if (healthValue.fillAmount != normalizeValue)
                healthValue.fillAmount = normalizeValue;

            _remapHealthColor.Change(healthValue.fillAmount);
        }

        internal void UpdateMagic()
        {
            float normalizeValue = playerData.magic / playerData.maxMagic;
            if (magicValue.fillAmount != normalizeValue)
                magicValue.fillAmount = normalizeValue;
        }

        #region StateMachine
        public override void OnEnter()
        {
            base.OnEnter();
            if(Application.isEditor == false)
                Cursor.visible = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (Application.isEditor == false)
                Cursor.visible = true;
        }
        #endregion
    }
}
