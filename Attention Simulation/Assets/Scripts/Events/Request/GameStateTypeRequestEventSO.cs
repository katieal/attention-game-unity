using UnityEngine;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "GameStateTypeRequestEventSO", menuName = "Events/Request/GameStateType")]
    public class GameStateTypeRequestEventSO : GenericGenericRequestEventSO<Managers.StateMachine.StateType, Managers.StateMachine.StateType> { }
}