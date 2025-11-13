using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Emyra.Simulator.Managers.StateMachine
{
    /// <summary>
    /// Pause all time-based operations during pause menu
    /// </summary>
    public class PauseState : MonoBehaviour, IGameState
    {
        #region Interface Fields
        public StateType Type { get; } = StateType.Pause;
        public List<Transition> Transitions { get; private set; } = new();

        public void SetEnabled(bool enabled) { this.enabled = enabled; }
        public void OnEnter()
        {
            this.enabled = true;
            PauseGame();
        }
        public void OnExit()
        {
            this.enabled = false;
            UnPauseGame();
            ResetState();
        }
        public void ResetState() { }
        #endregion

        private void PauseGame()
        {
            // freeze time
            Time.timeScale = 0f;

            // enable menu and ui actions and disable player actions 
            //InputSystem.actions.FindActionMap("Player").Disable();
            //InputSystem.actions.FindActionMap("Menu").Enable();
            //InputSystem.actions.FindActionMap("UI").Enable();
        }
        private void UnPauseGame()
        {
            // unfreeze time
            Time.timeScale = 1f;

            // re enable player actions
            //InputSystem.actions.FindActionMap("Player").Enable();
        }
    }
}
