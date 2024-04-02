using UnityEngine;
using Wcng;

namespace Wcng
{
    public class RunState : CharacterState
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
