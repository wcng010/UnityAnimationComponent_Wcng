using System;
using Animancer;
using UnityEngine;
using Wcng;

namespace Wcng.FSM
{
    [Serializable][CreateAssetMenu(fileName = "MoveState",menuName = "Data/State/MoveState")]
    public class MoveState : CharacterState
    {
        public override void OnPhysicUpdate()
        {
            
        }

        public override void OnLogicUpdate()
        {
            
        }

        public override void OnExit()
        {
           base.OnExit();
        }
    }
}
