using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Wcng;

namespace Wcng
{
    [Serializable] [CreateAssetMenu(fileName = "ComboState",menuName = "Data/State/ComboState")]
    public class ComboState : CharacterState
    {
        private bool _IsEnter;
        private bool _IsCombo;
        private Coroutine _CurrentCoroutine;
        public override void OnEnter()
        {
            _IsEnter = false;
            _IsCombo = false;
            //Controller.Play(animationBegin);
            Director.Play(timelineAsset);
            //当前段Animation已播完
            Controller.StartCoroutine(WaitForInvoke(() =>
            {
                //第一次连击
                if (_IsCombo)
                {
                    //Controller.Play(animationLoop);
                    Controller.StartCoroutine(WaitForInvoke(() =>
                    {
                            //第二次连击
                            if (_IsCombo)
                            {
                                //Controller.Play(animationEnd);
                                Controller.StartCoroutine(WaitForInvoke(() =>
                                {
                                    StateMachine.BackLastState();
                                }, animationEnd.Length));
                                _IsEnter = false;
                                _IsCombo = false;
                            }
                            else
                            {
                                StateMachine.BackLastState();
                            }
                            Controller.StopCoroutine(_CurrentCoroutine);
                            _IsEnter = false;
                            _IsCombo = false;
                    }, animationLoop.Length));
                }
                //不进行连击
                else
                {
                    StateMachine.BackLastState();
                }
                Controller.StopCoroutine(_CurrentCoroutine);
                _IsEnter = false;
                _IsCombo = false;
            }, animationBegin.Length));
        }
        public override void OnPhysicUpdate()
        {
            
        }
        public override void OnLogicUpdate()
        {
            //进入输入缓冲模式
            if (!_IsEnter)
            {
                AnimancerState currentState = Controller.States.Current;
                if ((currentState.Clip == animationBegin.Clip || currentState.Clip == animationLoop.Clip)&&Controller.States.Current.NormalizedTime >= 0.5)
                {
                    _IsEnter = true;
                    _CurrentCoroutine = Controller.StartCoroutine(ComboStateInputBuffer());
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        //循环检测输入
        private IEnumerator ComboStateInputBuffer()
        {
            while (true)
            {
                if (InputComponent.GetPressedKeys().Contains(InputKey.MouseClickLeft))
                {
                    _IsCombo = true;
                }
                yield return null;
            }
        }
    }
}
