using UnityEngine;

namespace State
{
    public interface IStateEnter
    {
        public void OnEnter();
    }

    public interface IStateExit
    {
        public void OnExit();
    }

    public class Substate : MonoBehaviour
    {
        private bool isDebug = false;

        private SimpleStateMachine _myStateMachine;
        private SimpleStateMachine MyStateMachine
        {
            get
            {
                if (_myStateMachine == null)
                    _myStateMachine = GetComponentInParent<SimpleStateMachine>();
                return _myStateMachine;
            }
        }

        private IStateEnter[] _statesEnter;
        private IStateEnter[] StatesEnter
        {
            get => GetIStates(ref _statesEnter);
        }

        private IStateExit[] _statesExit;
        private IStateExit[] StatesExit
        {
            get => GetIStates(ref _statesExit);
        }

        private T[] GetIStates<T>(ref T[] states)
        {
            if (states == null)
                states = GetComponentsInChildren<T>();
            return states;
        }

        [ExposeMethodInEditor]
        public void Enter() => MyStateMachine.Enter(this);

        internal void OnEnter()
        {
            foreach (IStateEnter item in StatesEnter)
                item.OnEnter();
        }

        internal void OnExit()
        {
            foreach (IStateExit item in StatesExit)
                item.OnExit();
        }

        public void EnterLastState() => MyStateMachine.EnterLast();

        private void PrintStateName()
        {
            if(isDebug)
                Debug.Log($"Enter state: {gameObject.name}", this);
        }
    }
}
