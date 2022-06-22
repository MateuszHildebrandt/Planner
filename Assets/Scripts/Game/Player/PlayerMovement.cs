using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerMovement : MonoBehaviour, Game.IInputActionsReceiver
    {
        [Header("Settings")]
        [SerializeField] float speed = 4f;
        [Header("Resources")]
        [SerializeField] PlayerData playerData;

        private Rigidbody2D _rigidbody2d;
        private Animator _animator;
        private InputActions _inputActions;
        private Vector2 _direction;

        internal Vector2 GetDirection() => _direction;

        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Move(_inputActions.Player.Move.ReadValue<Vector2>());
        }

        private void FixedUpdate()
        {
            if (playerData.health <= 0)
                return;

            if (_direction != Vector2.zero)
                _rigidbody2d.MovePosition(_rigidbody2d.position + _direction * speed * Time.fixedDeltaTime);
        }

        internal void Move(Vector2 position)
        {
            if (playerData.health <= 0)
            {
                _animator.speed = 0f;
                return;
            }

            _direction = new Vector2(position.x, position.y);

            _animator.SetFloat("Horizontal", _direction.x);
            _animator.SetFloat("Vertical", _direction.y);
            _animator.SetFloat("Speed", _direction.sqrMagnitude);

            if (_direction == Vector2.zero)
                _animator.speed = 0f;
            else
                _animator.speed = 1f;

            playerData.position = transform.position;
        }

        public void SetupInputActions(InputActions input) => _inputActions = input;
    }
}
