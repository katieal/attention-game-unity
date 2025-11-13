using Emyra.Simulator.EventChannel;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Emyra.Simulator.Managers.StateMachine
{
    /// <summary>
    /// Starting state for the game
    /// </summary>
    public class StartState : MonoBehaviour, IGameState
    {
        [Title("Listening to Events")]
        [SerializeField] private VoidEventSO _startNewGameEvent;

        #region Interface Fields
        public StateType Type { get; } = StateType.Start;
        public List<Transition> Transitions { get; private set; } = new();

        public void SetEnabled(bool enabled) { this.enabled = enabled; }
        public void OnEnter() 
        { 
            this.enabled = true; 
        }
        public void OnExit() 
        { 
            this.enabled = false;
            ResetState();
        }
        public void ResetState() { }
        #endregion

        private bool _isGameStarting = false;

        private void Awake()
        {
            Transitions.Add(new Transition(StateType.Run, () => _isGameStarting == true));
        }

        private void OnEnable()
        {
            _startNewGameEvent.OnInvokeEvent += OnStartNewGameEvent;
        }
        private void OnDisable()
        {
            _startNewGameEvent.OnInvokeEvent -= OnStartNewGameEvent;
        }

        #region Event Callbacks
        private void OnStartNewGameEvent() { StartNewGameAsync().Forget(); }
        #endregion

        private async UniTaskVoid StartNewGameAsync()
        {
            // loading functions and stuff go here
            await UniTask.WaitForEndOfFrame();

            _isGameStarting = true;
        }

        private void LoadGame()
        {
            // call load state instead ?
        }
    }
}
