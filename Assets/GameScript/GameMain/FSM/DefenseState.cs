using System;
using System.Collections;
using Animancer;
using UnityEngine;
using Wcng;

namespace Wcng
{
    [Serializable]
    public class DefenseState : CharacterState
    {
        public override bool CanChangeState => canChangeState;
        
        [SerializeField]private bool canChangeState = false;

        private AnimancerState _AnimancerState;

        private int _EnterCount = 0;

        public override void OnEnter()
        {
            if (AnimationBegin != null)
            {
                Controller.Play(AnimationBegin);
                _AnimancerState = Controller.States.Current;
                ++_EnterCount;
                if (_EnterCount == Int32.MaxValue) _EnterCount = 0;
                Controller.StartCoroutine(WaitForInvoke((enterCount) =>
                {
                    if(Controller.States.Current == _AnimancerState && enterCount == _EnterCount)
                        Controller.Play(AnimationLoop);
                }, Controller.States.Current.Length,_EnterCount));
            }
        }

        public override void OnPhysicUpdate()
        { 
            
        }

        public override void OnLogicUpdate()
        {
            if (!InputComponent.GetPressedKeys().Contains(InputKey.MouseClickRight))
            {
                if (Controller.States.Current.Clip != AnimationEnd.Clip)
                {
                    Controller.Play(AnimationEnd);
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
