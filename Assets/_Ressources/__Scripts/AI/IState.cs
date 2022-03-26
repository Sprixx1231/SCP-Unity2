using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public enum StateID
    {
        Chase, Idle, Walk, Vocalize, Attack, Death
    }
    public interface IState
    {
        StateID GetStateID();
        void Enter(Agent agent);
        void Update(Agent agent);
        void Exit(Agent agent);
        
    }
}
