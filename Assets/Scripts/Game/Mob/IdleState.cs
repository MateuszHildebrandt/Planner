using UnityEngine;

namespace Mob
{
    public class IdleState : MobStateBase, IMobState
    {
        [Header("References")]
        [SerializeField] Transform[] waypoints;

        private int _nextWaypoint = 0;

        public void OnTriggerEnter(Collider other) //Target is too close
        {
            if (MyMobController.CompareWithTags(other.tag))
            {
                MyMobController.target = other.transform;
                MyMobController.ToAlertState();
            }
        }

        public void Initialize()
        {
        }

        public void OnStartState()
        {
            
        }

        public void UpdateState()
        {
            Idle();
        }

        private void Idle()
        {
            if (MyMobController.Sight.GetPlayerObserved())
            {
                MyMobController.ToAlertState();
            }
            else
            {
                if (waypoints.Length <= 0)
                    return;

                MyMobController.Agent.destination = waypoints[_nextWaypoint].position;
                MyMobController.Agent.isStopped = false;

                if (MyMobController.Agent.remainingDistance <= MyMobController.Agent.stoppingDistance && MyMobController.Agent.pathPending == false)
                    _nextWaypoint = (_nextWaypoint + 1) % waypoints.Length;
            }
        }
    }
}
