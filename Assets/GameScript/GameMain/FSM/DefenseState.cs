using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using Wcng;

namespace Wcng.FSM
{
    [Serializable] [CreateAssetMenu(fileName = "DefenseState",menuName = "Data/State/DefenseState")]
    public class DefenseState : CharacterState
    {

        private int _enterCount = 0;
        private List<ClipTransition> _clips;
        public override void OnEnter()
        {
            _clips = new List<ClipTransition>(AnimationClips.Keys);
            Controller.Play(_clips[0]);
            if (_clips[0] != null)
            {
                ++_enterCount;
                if (_enterCount == Int32.MaxValue) _enterCount = 0;
                Controller.StartCoroutine(WaitForInvoke((enterCount) =>
                {
                    if(Controller.States.Current.Clip == _clips[0].Clip && enterCount == _enterCount)
                        Controller.Play(_clips[1]);
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
                if (Controller.States.Current.Clip != _clips[2].Clip)
                {
                    Controller.Play(_clips[2]);
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
