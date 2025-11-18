using Emyra.FocusGame.EventChannel;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

namespace Emyra.FocusGame.Managers.StateMachine
{
    public class GameStateMachine : SerializedMonoBehaviour
    {
        // dictionary of all states, assigned in inspector
        [Title("State Dictionary")]
        [SerializeField]
        private Dictionary<StateType, IGameState> _stateDict = new Dictionary<StateType, IGameState>()
        {
            { StateType.Start, null },
            { StateType.Load, null },
            { StateType.Pause, null },
            { StateType.Run, null }
        };

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Responding to Events")]
        [SerializeField] private GameStateTypeRequestEventSO _changeStateEvent;

        [Header("Starting State")]
        [SerializeField] private StateType _currentState;

        private void Awake()
        {
            if (_currentState == StateType.None) { _currentState = StateType.Start; }

            // all states start disabled
            foreach (StateType type in _stateDict.Keys)
            {
                _stateDict[type]?.SetEnabled(false);
            }
        }

        private void OnEnable()
        {
            _changeStateEvent.OnRequestEvent += OnChangeState;
        }
        private void OnDisable()
        {
            _changeStateEvent.OnRequestEvent -= OnChangeState;
        }

        private void Start()
        {
            // enable starting state
            _stateDict[_currentState].OnEnter();

            // broadcast changed state
            _changeStateEvent.SendResult(_currentState);
        }

        private void Update()
        {
            foreach (Transition transition in _stateDict[_currentState].Transitions)
            {
                if (transition.Condition()) { OnChangeState(transition.NewState); }
            }
        }

        private void OnChangeState(StateType newState)
        {
            // return if game is already in this state
            if (_currentState == newState) { return; }

            // exit current state
            _stateDict[_currentState].OnExit();

            // enable new state
            _currentState = newState;
            _stateDict[_currentState].OnEnter();

            // broadcast changed state
            _changeStateEvent.SendResult(_currentState);
        }
    }
}