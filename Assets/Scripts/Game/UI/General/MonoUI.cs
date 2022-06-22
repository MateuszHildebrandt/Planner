
namespace UI
{
    [UnityEngine.RequireComponent(typeof(State.Substate))]
    public abstract class MonoUI : UnityEngine.MonoBehaviour, State.IStateEnter, State.IStateExit
    {
        [ReadOnly, UnityEngine.SerializeField] bool _isActive;

        private State.Substate _myState;
        protected State.Substate MyState
        {
            get
            {
                if (_myState == null)
                    _myState = GetComponent<State.Substate>();
                return _myState;
            }
        }

        internal bool IsActive() => _isActive;

        public void EnterState() => MyState.Enter();

        public virtual void OnEnter() => _isActive = true;

        public virtual void OnExit() => _isActive = false;
    }
}
