using System;
using System.Collections;
using Animancer;
using UnityEngine;
using Wcng;

namespace Wcng
{
    [Serializable] [CreateAssetMenu(fileName = "DefenseState",menuName = "Data/State/DefenseState")]
    public class DefenseState : CharacterState
    {

        private int _enterCount = 0;

        public override void OnEnter()
        {
            Controller.Play(animationBegin);
            if (animationBegin != null)
            {
                ++_enterCount;
                if (_enterCount == Int32.MaxValue) _enterCount = 0;
                Controller.StartCoroutine(WaitForInvoke((enterCount) =>
                {
                    if(Controller.States.Current.Clip == animationBegin.Clip && enterCount == _enterCount)
                        Controller.Play(animationLoop);
                }, Controller.States.Current.Length,_enterCount));
            }
        }

        public override void OnPhysicUpdate()
        { 
            
        }

        public override void OnLogicUpdate()
        {
            if (!InputComponent.GetPressedKeys().Contains(InputKey.MouseClickRight))
            {
                if (Controller.States.Current.Clip != animationEnd.Clip)
                {
                    Controller.Play(animationEnd);
                    Controller.StartCoroutine(WaitForInvoke(() => {StateMachine.BackLastState(); },
                        Controller.States.Current.Length));
                }
            }
        }

        public override void OnExit()
        {
            
        }
    }
}
