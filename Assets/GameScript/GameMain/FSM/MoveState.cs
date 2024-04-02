using System;
using Animancer;
using UnityEngine;
using Wcng;

namespace Wcng
{
    [Serializable]
    public class MoveState : CharacterState
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
