using Emyra.Simulator.EventChannel;
using UnityEngine;

namespace Emyra.Simulator.Testing
{
    public class DebugManager : MonoBehaviour
    {
        public GameStateTypeRequestEventSO ChangeStateEvent;


        private void OnEnable()
        {
            ChangeStateEvent.OnRequestEvent += PrintState;
        }
        private void OnDisable()
        {
            ChangeStateEvent.OnRequestEvent -= PrintState;
        }

        private void PrintState(Managers.StateMachine.StateType type)
        {
            Debug.Log(type);
        }
    }
}
