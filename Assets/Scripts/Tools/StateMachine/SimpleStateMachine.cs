using UnityEngine;

namespace State
{
    public class SimpleStateMachine : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Substate defaultState;

        [Header("Debug")]
        [ReadOnly, SerializeField] Substate currState;
        [ReadOnly, SerializeField] Substate lastState;

        private void Start()
        {
            if (defaultState != null)
                defaultState.Enter();
        }

        internal void Enter(Substate newState)
        {
            if (newState == null)
                return;

            if (newState == currState)
                return;

            currState?.OnExit();

            lastState = currState;
            currState = newState;

            currState.OnEnter();
        }

        internal void EnterLast()
        {
            if (lastState != null)
                Enter(lastState);
        }
    }
}
