using UnityEngine;

namespace Mob
{
    public class SearchState : MobStateBase, IMobState
    {
        [SerializeField] float waitTime = 5f;

        private float timer;     

        public void Initialize()
        {
            MyMobController.onDamage += () => MyMobController.ToAlertState();
        }

        public void OnStartState()
        {
            timer = 0;
            MyMobController.Agent.destination = MyMobController.target.position;
        }

        public void UpdateState()
        {
            Search();
        }

        private void Search()
        {
            if (MyMobController.Agent.remainingDistance <= MyMobController.Agent.stoppingDistance)
            {
                timer += Time.deltaTime;

                if (timer >= waitTime)
                    MyMobController.ToIdleState();
                else if (MyMobController.Sight.GetPlayerObserved())
                    MyMobController.ToAlertState();
            }
        }
    }
}
