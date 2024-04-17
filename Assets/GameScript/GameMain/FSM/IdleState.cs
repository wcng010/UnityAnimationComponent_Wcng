using System;
using Animancer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Wcng.FSM
{
    [Serializable][CreateAssetMenu(fileName = "IdleState",menuName = "Data/State/IdleState")]
    public class IdleState : CharacterState
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
