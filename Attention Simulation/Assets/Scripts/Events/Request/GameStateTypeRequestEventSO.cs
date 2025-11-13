using UnityEngine;

namespace Emyra.Simulator.EventChannel
{
    [CreateAssetMenu(fileName = "GameStateTypeRequestEventSO", menuName = "Events/Request/TwoArg/GameStateType")]
    public class GameStateTypeRequestEventSO : GenericGenericRequestEventSO<Managers.StateMachine.StateType, Managers.StateMachine.StateType> { }
}