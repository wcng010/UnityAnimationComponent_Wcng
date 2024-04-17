using System;
using UnityEngine;
using Wcng;

namespace Wcng.FSM
{
    [Serializable][CreateAssetMenu(fileName = "RunState",menuName = "Data/State/RunState")]
    public class RunState : CharacterState
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
