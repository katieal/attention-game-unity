using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.Managers.StateMachine
{
    /// <summary>
    /// Game state during standard gameplay
    /// </summary>
    public class RunState : MonoBehaviour, IGameState
    {
        #region Interface Fields
        public StateType Type { get; } = StateType.Run;
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
    }
}
