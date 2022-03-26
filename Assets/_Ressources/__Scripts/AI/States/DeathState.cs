using UnityEngine;
using UnityEngine.UI;

namespace Game.AI.States
{
    public class DeathState : IState
    {
        public StateID GetStateID()
        {
            return StateID.Death;
        }

        public void Enter(Agent agent)
        {
            //TODO: Implement ragdoll physics
            /*
            agent.ragdoll.ActivateRagdoll();
            direction.y = 1;
            ragdoll.ApplyForce(direction * agent.config.DieForce);
            healthbar.gameObject.SetActive(false);
            SkinnedMeshRenderer.updateWhenOffScreen = true;
            */
        }

        public void Update(Agent agent)
        {
        }

        public void Exit(Agent agent)
        {
        }
    }
}