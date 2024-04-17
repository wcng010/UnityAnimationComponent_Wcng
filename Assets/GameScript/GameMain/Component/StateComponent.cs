using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using Wcng.FSM;

namespace Wcng.Component
{
    public class StateComponent : SerializedMonoBehaviour
    {
        
        [FoldoutGroup("Animation Module")] [SerializeField] 
        private IState _OriginalState;
        [FoldoutGroup("Animation Module")] 
        public Dictionary<List<InputKey>, CharacterState> AnimationStates = new Dictionary<List<InputKey>, CharacterState>();
        
        
        [FoldoutGroup("Reference")] [SerializeField] 
        private AnimancerComponent controller;
        
        [FoldoutGroup("Reference")] [SerializeField]
        private InputComponent inputComponent;

        [FoldoutGroup("Reference")] [SerializeField]
        private PlayableDirector playableDirector;
        
        private CharacterStateMachine _StateMachine;

        private void Awake()
        {
            _StateMachine = new CharacterStateMachine(inputComponent,controller,playableDirector);
            _StateMachine.ChangeState(_OriginalState);
        }
        
        private void Update()
        {
            AnimationPlay();
            _StateMachine.LogicUpdate();
        }

        private void FixedUpdate()
        {
            _StateMachine.FixedUpdate();
        }
        
        //遍历输入键Bool对，如果输入对复合当前状态需求则进入状态。
        //如果没有输入则，要求
        private void AnimationPlay()
        {
            if (inputComponent)
            {
                //遍历输入组件的输入键
                List<InputKey> pressedKeys = inputComponent.GetPressedKeys();
                int pressedKeyNum = pressedKeys.Count;
                if (pressedKeyNum == 0)
                    _StateMachine.ChangeState(_OriginalState);
                //遍历每一条指令的输入条件                          
                foreach (var animKeys in AnimationStates)
                {
                    int count = 0;
                    if (pressedKeyNum != animKeys.Key.Count)
                        continue;
                    foreach (var key in animKeys.Key)
                    {
                        //如果当前压入键包含当前条件
                        if (pressedKeys.Contains(key))
                        {
                            ++count;
                        }
                    }

                    if (count == animKeys.Key.Count)
                    {
                        _StateMachine.ChangeState(animKeys.Value);
                    }
                }
            }
        }

        public IState GetCurrentState() => _StateMachine.CurrentState;

    }
}
