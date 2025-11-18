using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.Managers.StateMachine
{
    public enum StateType { None = -1, Start = 0, Load, Pause, Run }

    public struct Transition
    {
        public StateType NewState;
        public Func<bool> Condition;
        public Transition(StateType newState, Func<bool> condition)
        {
            NewState = newState;
            Condition = condition;
        }
    }

    public interface IGameState
    {
        public StateType Type { get; }
        public List<Transition> Transitions { get; }

        public void SetEnabled(bool enabled);
        public void OnEnter();
        public void OnExit();
        public void ResetState();
    }
}
