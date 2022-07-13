using Game;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Mob
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(SpriteRenderer))]
    public class MobController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] MobData mobData;
        [Header("Settings")]
        [SerializeField] float health = 30;
        [Space]
        [SerializeField] bool canShoot = true;
        [SerializeField, IfHide(nameof(canShoot))] float shootDamage = 10;
        [SerializeField, IfHide(nameof(canShoot))] float range = 4f;
        [Space]
        [SerializeField] bool canMeleeAttack = true;    
        [SerializeField, IfHide(nameof(canMeleeAttack))] int meleeDamage = 20;
        [Space]
        [SerializeField] string[] respondForTags;

        internal Action onDamage;

        private IMobState _currentState;
        private IdleState _idleState;
        private AttackState _attackState;
        private SearchState _searchState;
        private AlertState _alertState;
        private Animator _animator;
        private MobShooting _mobShooting;

        internal bool IsLive { get; private set; } = true;
        internal NavMeshAgent Agent { get; private set; }
        internal MobSight Sight { get; private set; }
        internal Transform target { get; set; }
        internal float GetRange() => range;
        internal Vector2 Direction { get; private set; }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;

            Sight = GetComponentInChildren<MobSight>();
            _mobShooting = GetComponent<MobShooting>();

            _idleState = GetComponent<IdleState>();
            _attackState = GetComponent<AttackState>();
            _searchState = GetComponent<SearchState>();
            _alertState = GetComponent<AlertState>();
            _animator = GetComponent<Animator>();

            _currentState = _idleState;
        }

        private void OnEnable()
        {
            GameController.I.RegisterMob(mobData, gameObject);
            //Debug.Log($"Register mob: {mobData.id}", this);
        }

        private void Update()
        {
            if (IsLive)
            {
                _currentState.UpdateState();
                UpdateAnimations();

                if(mobData.position.x != transform.position.x || mobData.position.y != transform.position.y)
                    mobData.position = transform.position;
            }
        }

        private void UpdateAnimations()
        {
            Direction = Agent.velocity;
            Direction.Normalize();

            _animator.SetFloat("Horizontal", Direction.x);
            _animator.SetFloat("Vertical", Direction.y);
            _animator.SetFloat("Speed", Direction.sqrMagnitude);

            if (Direction == Vector2.zero)
                _animator.speed = 0f;
            else
                _animator.speed = 1f;
        }

        internal void Attack()
        {
            if(canShoot)
                _mobShooting.Shoot(shootDamage);
            if (canMeleeAttack)
                ;//TODO
        }

        internal void Damage(float value)
        {
            health -= value;

            if (health <= 0)
                Kill();

            onDamage?.Invoke();
        }

        private void Kill()
        {
            IsLive = false;
            Agent.isStopped = true;
            _animator.SetFloat("Speed", 0);
            GetComponent<SpriteRenderer>().color = Color.red;
            mobData.live = false;
            Destroy(gameObject, 1f);
        }

        internal void Load(MobData data)
        {
            if (data != null)
                mobData = data;
            Setup(mobData.live);
        }

        internal void Setup(bool live)
        {
            mobData.live = live;
            gameObject.SetActive(live);
        }

        internal bool CompareWithTags(string currentTag)
        {
            foreach (string respondTag in respondForTags)
            {
                if (currentTag == respondTag)
                    return true;
            }

            return false;
        }

        internal IMobState CurrenetState
        {
            get { return _currentState; }
            private set
            {
                _currentState = value;
                _currentState?.OnStartState();
            }
        }

        internal void ToIdleState() => CurrenetState = _idleState;
        internal void ToAttackState() => CurrenetState = _attackState;
        internal void ToSearchState() => CurrenetState = _searchState;
        internal void ToAlertState() => CurrenetState = _alertState;

        [ContextMenu("GenerateId")]
        private void GenerateId() => mobData.id = Guid.NewGuid().ToString();
    }
}
