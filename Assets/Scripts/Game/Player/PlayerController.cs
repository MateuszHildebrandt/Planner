using UnityEngine;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] bool isActive;
        [Header("Resources")]
        [SerializeField] PlayerData playerData;

        private SpriteRenderer _spriteRenderer;
        private InputActions _inputActions;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _inputActions.UI.Escape.performed += (_) => IsActive = !IsActive;
        }

        private void OnEnable()
        {
            IsActive = true;
        }

        private void OnDisable()
        {
            IsActive = false;
        }        

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }

        [Inject]
        private void Construct(InputActions inputActions)
        {
            _inputActions = inputActions;
        }

        private bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                if (value)
                    _inputActions.Player.Enable();
                else
                    _inputActions.Player.Disable();
            }
        }

        internal void Load()
        {
            transform.position = playerData.position;
        }

        internal void Damage(float value)
        {
            playerData.ClampHealth(playerData.health - value);

            if (playerData.health <= 0)
                Kill();
            else
                _spriteRenderer.color = Color.white;
        }

        private void Kill()
        {
            _spriteRenderer.color = Color.red;            
        }     
    }
}
