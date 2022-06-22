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

        private IMobState currentState;
        private IdleState idleState;
        private AttackState attackState;
        private SearchState searchState;
        private AlertState alertState;
        private Animator animator;
        private MobShooting mobShooting;

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
            mobShooting = GetComponent<MobShooting>();

            idleState = GetComponent<IdleState>();
            attackState = GetComponent<AttackState>();
            searchState = GetComponent<SearchState>();
            alertState = GetComponent<AlertState>();
            animator = GetComponent<Animator>();

            currentState = idleState;
        }

        private void OnEnable()
        {
            GameController.I.RegisterMob(mobData, gameObject);
            Debug.Log($"Register mob: {mobData.id}", this);
        }

        private void Update()
        {
            if (IsLive)
            {
                currentState.UpdateState();
                UpdateAnimations();

                if(mobData.position.x != transform.position.x || mobData.position.y != transform.position.y)
                    mobData.position = transform.position;
            }
        }

        private void UpdateAnimations()
        {
            Direction = Agent.velocity;
            Direction.Normalize();

            animator.SetFloat("Horizontal", Direction.x);
            animator.SetFloat("Vertical", Direction.y);
            animator.SetFloat("Speed", Direction.sqrMagnitude);

            if (Direction == Vector2.zero)
                animator.speed = 0f;
            else
                animator.speed = 1f;
        }

        internal void Attack()
        {
            if(canShoot)
                mobShooting.Shoot(shootDamage);
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
            animator.SetFloat("Speed", 0);
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
            get { return currentState; }
            private set
            {
                currentState = value;
                currentState?.OnStartState();
            }
        }

        internal void ToIdleState() => CurrenetState = idleState;
        internal void ToAttackState() => CurrenetState = attackState;
        internal void ToSearchState() => CurrenetState = searchState;
        internal void ToAlertState() => CurrenetState = alertState;

        [ContextMenu("GenerateId")] //TODO move to one script
        private void GenerateId()
        {
            Guid guid = Guid.NewGuid();
            mobData.id = guid.ToString();
        }
    }
}
