
namespace Mob
{
    public interface IMobState
    {
        void Initialize();
        void OnStartState();
        void UpdateState();
    }

    public abstract class MobStateBase : UnityEngine.MonoBehaviour
    {
        private MobController _myMobController;
        protected MobController MyMobController
        {
            get
            {
                if (_myMobController == null)
                    _myMobController = GetComponent<MobController>();
                return _myMobController;
            }
        }
    }
}
