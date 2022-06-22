using UnityEngine;

namespace Mob
{
    public class AttackState : MobStateBase, IMobState
    {
        public void Initialize()
        {
        }

        public void OnStartState()
        {

        }

        public void UpdateState()
        {
            Attack();
        }

        private void Attack()
        {
            if (MyMobController.Sight.GetPlayerObserved())
            {
                MyMobController.Agent.SetDestination(transform.position);
                MyMobController.Attack();
            }
            else
                MyMobController.ToAlertState();
        }
    }
}
