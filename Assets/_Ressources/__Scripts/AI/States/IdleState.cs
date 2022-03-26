using UnityEngine;

namespace Game.AI.States
{
    public class IdleState : IState
    {
        public StateID GetStateID()
        {
            return StateID.Idle;
        }

        public void Enter(Agent agent)
        {
           
        }

        public void Update(Agent agent)
        {
            var playerDir = agent.playerTransform.position - agent.transform.position;
            if (playerDir.magnitude > agent.config.MaxSightDistance)
            {
                return;
            }

            var agentDir = agent.transform.forward;
            
            playerDir.Normalize();

            var dotProduct = Vector3.Dot(playerDir, agentDir);
            if (dotProduct > 0.0f)
            {
                agent.StateMachine.ChangeState(StateID.Chase);
            }

        }

        public void Exit(Agent agent)
        {
           
        }
    }
}