using System;
using Game.AI.States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Game.AI
{
    public class Agent : MonoBehaviour
    {
        public StateMachine StateMachine; 
        [SerializeField] private StateID initialState; 
        public NavMeshAgent navMeshAgent; 
        public NpcConfig config;
        //public Ragdoll ragdoll;
        public Transform playerTransform;
        public SkinnedMeshRenderer mesh;
        
        

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            mesh = GetComponent<SkinnedMeshRenderer>();
            StateMachine = new StateMachine(this);
            StateMachine.RegisterState(new ChasePlayerState());
            StateMachine.RegisterState(new DeathState());
            StateMachine.RegisterState(new IdleState());
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            StateMachine.ChangeState(initialState);
        }

        private void Update()
        {
            StateMachine.Update();
        }
    }
}