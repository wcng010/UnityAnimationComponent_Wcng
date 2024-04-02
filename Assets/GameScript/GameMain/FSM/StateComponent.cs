using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Wcng
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
        
        private CharacterStateMachine _StateMachine;

        private void Awake()
        {
            _StateMachine = new CharacterStateMachine(inputComponent,controller);
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
            //遍历输入组件的输入键
            List<InputKey> pressedKeys= inputComponent.GetPressedKeys();
            if (pressedKeys.Count == 0) 
                _StateMachine.ChangeState(_OriginalState);
            foreach (var animKeys in AnimationStates)
            {
                int count = 0;
                foreach (var key in animKeys.Key)
                {
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

        public IState GetCurrentState() => _StateMachine.CurrentState;

    }
}
