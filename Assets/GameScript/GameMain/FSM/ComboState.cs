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
        public override bool CanChangeState => canChangeState;
        
        [SerializeField]private bool canChangeState = false;
        private bool _IsEnter;
        private bool _IsCombo;
        private Coroutine _CurrentCoroutine;
        public override void OnEnter()
        {
            _IsEnter = false;
            _IsCombo = false;
            Controller.Play(AnimationBegin);
            //当前段Animation已播完
            Controller.StartCoroutine(WaitForInvoke(() =>
            {
                //如果进行连击指令
                if (_IsCombo)
                {
                    if (Controller.States.Current.Clip == AnimationBegin.Clip)
                    {
                        Controller.Play(AnimationLoop);
                        Controller.StartCoroutine(WaitForInvoke(() =>
                        {
                            if (_IsCombo)
                            {
                                Controller.Play(AnimationEnd);
                                Controller.StartCoroutine(WaitForInvoke(() =>
                                {
                                    StateMachine.BackLastState();
                                }, Controller.States.Current.Length));
                            }
                            else
                            {
                                StateMachine.BackLastState();
                            }
                            Controller.StopCoroutine(_CurrentCoroutine);
                            _IsEnter = false;
                            _IsCombo = false;
                        }, Controller.States.Current.Length));
                        Controller.StopCoroutine(_CurrentCoroutine);
                        _IsEnter = false;
                        _IsCombo = false;
                    }
                }
                //不进行连击
                else
                {
                    StateMachine.BackLastState();
                }
                //停止输入检测
                Controller.StopCoroutine(_CurrentCoroutine);
                _IsEnter = false;
                _IsCombo = false;
            }, Controller.States.Current.Length));
        }

        public override void OnPhysicUpdate()
        {
            
        }

        public override void OnLogicUpdate()
        {
            //进入输入缓冲模式
            if (Controller.States.Current.NormalizedTime >= 0.5f&&!_IsEnter)
            {
                _IsEnter = true;
                _CurrentCoroutine = Controller.StartCoroutine(ComboStateInputBuffer());
            }
        }

        public override void OnExit()
        {
            
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
