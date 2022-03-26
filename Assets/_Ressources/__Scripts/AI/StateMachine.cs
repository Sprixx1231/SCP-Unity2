using UnityEngine;

namespace Game.AI
{
    public class StateMachine
    {
        private IState[] _states;
        public Agent _agent;
        private StateID _currentState; 

        public StateMachine(Agent agent)
        {
            this._agent = agent;
            var numStates = System.Enum.GetNames(typeof(StateID)).Length;
            _states = new IState[numStates];
        }

        public void RegisterState(IState state)
        {
            var index = (int)state.GetStateID();
            _states[index] = state;
        }

        public IState GetStateID(StateID stateID)
        {
            var index = (int)stateID;
            return _states[index];
        }

        public void Update()
        {
            GetStateID(_currentState)?.Update(_agent);
            
        }

        public void ChangeState(StateID newState)
        {
            GetStateID(_currentState)?.Exit(_agent);
            _currentState = newState;
            GetStateID(_currentState)?.Enter(_agent);
        }
    }
}