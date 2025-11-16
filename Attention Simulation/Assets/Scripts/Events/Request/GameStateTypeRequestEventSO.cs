using UnityEngine;

namespace Emyra.Simulator.EventChannel
{
    [CreateAssetMenu(fileName = "GameStateTypeRequestEventSO", menuName = "Events/Request/GameStateType")]
    public class GameStateTypeRequestEventSO : GenericGenericRequestEventSO<Managers.StateMachine.StateType, Managers.StateMachine.StateType> { }
}