using UnityEngine;
using UnityEngine.AI;

namespace Game.AI.States
{
    public class ChasePlayerState : IState
    { 

        private float _timer = 0.0f;


        public StateID GetStateID()
        {
            return StateID.Chase;
        }

        public void Enter(Agent agent)
        {
            if (agent.playerTransform == null)
            {
               
            }
        }

        public void Update(Agent agent)
        {
            if (!agent.enabled) return;

            _timer -= Time.deltaTime;
            if (agent.navMeshAgent.hasPath)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }

            if (_timer < 0.0f)
            {
                Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
                direction.y = 0;
                if (!(direction.sqrMagnitude > agent.config.MaxDistance * agent.config.MaxDistance)) return;
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }

                _timer = agent.config.MaxTime;
            }

            /* if (agent.hasPath)
             {
                 //TODO: Set Animator bool
             } */
        }

        public void Exit(Agent agent)
        {
           Debug.Log("bye");
        }
    }
    
}
