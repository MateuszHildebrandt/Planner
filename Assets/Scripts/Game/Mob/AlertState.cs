using UnityEngine;

namespace Mob
{
    public class AlertState : MobStateBase, IMobState
    {
        private Vector3 lastKnownPosition;

        public void Initialize()
        {
        }

        public void OnStartState()
        {
        }

        public void UpdateState()
        {
            Alert();
        }

        private void Alert()
        {
            if (MyMobController.Sight.GetPlayerObserved())
            {
                lastKnownPosition = MyMobController.target.position;

                //Enemy get info about remaining distance.
                if (MyMobController.Agent.remainingDistance <= MyMobController.GetRange())
                    MyMobController.ToAttackState();
                else
                {
                    Debug.DrawLine(MyMobController.target.position, MyMobController.target.position + new Vector3(0.1f, 0.1f, 0.1f), Color.red, 3);
                    MyMobController.Agent.destination = MyMobController.target.position;
                    MyMobController.Agent.isStopped = false;
                }
            }
            else
            {
                MyMobController.ToSearchState();
            }
        }
    }
}
