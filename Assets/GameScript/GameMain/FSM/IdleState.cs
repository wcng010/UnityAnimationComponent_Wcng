using System;
using Animancer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Wcng
{
    [Serializable]
    public class IdleState : CharacterState
    {
        public override bool CanChangeState => canChangeState;
        public override void OnPhysicUpdate()
        {
            
        }

        public override void OnLogicUpdate()
        {
           
        }

        public override void OnExit()
        {
            
        }

        [SerializeField]private bool canChangeState = true;
    }
}
