using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Animancer;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static Wcng.FunctionLibrary;
namespace Wcng
{
    [Serializable]
    public enum State
    {
        Move,
        Attack,
    }
    public class AnimationPreformComponent: SerializedMonoBehaviour
    {
        [Title("角色移动方向")]
        public Vector2 moveVector2;
        [Title("CameraForward修正方向")] 
        public Vector3 moveForwardVector3;
        [FoldoutGroup("动画")] [Title("持续动画")]
        public Dictionary<InputAction, ClipTransition> AnimationSustain = new Dictionary<InputAction, ClipTransition>();
        [FoldoutGroup("动画")] [Title("瞬时动画")]
        public Dictionary<InputAction, ClipTransition> AnimationInstant = new Dictionary<InputAction, ClipTransition>();
        [SerializeField] [Title("原始动画")]
        private ClipTransition originalState;
        [Title("输入")]
        public Dictionary<string, InputAction> InputActions = new Dictionary<string, InputAction>();
        [SerializeField]
        private AnimancerComponent controller;

        public Dictionary<State, ClipTransition> AnimationStates = new Dictionary<State, ClipTransition>();

        [SerializeField]private InputComponent inputComponent;
        //[SerializeField] 
        //private ClipTransition moveState;
        private void Awake()
        {
            /*
            controller.Play(originalState);
            foreach (var key in AnimationInstant.Keys)
            {
                Debug.Log(key);
                key.Enable();
                key.started += OnInputStartInstant;
            }

            foreach (var key in AnimationSustain.Keys)
            {
                Debug.Log(key);
                key.Enable();
                key.started += OnInputStartSustain;
                key.performed += OnInputPerformSustain;
                key.canceled += OnInputEndSustain;
            }*/
        }

        private void Update()
        {
            moveForwardVector3 = ConvertMoveInputToCameraSpace(Camera.main.transform, moveVector2.x, moveVector2.y);
            AnimationPlay();
        }


        private void AnimationPlay()
        {
            foreach (var inputAction in inputComponent.InputActions.Keys)
            {
                
            }
        }






        //瞬时技能
        private void OnInputStartInstant(InputAction.CallbackContext obj)
        {
            ClipTransition animationClip = AnimationInstant[obj.action];
            controller.Play(animationClip);
            Invoke(nameof(ReturnOrignalState),animationClip.Length);
        }
        //持久技能触发
        private void OnInputStartSustain(InputAction.CallbackContext obj)
        {
            controller.Play(AnimationSustain[obj.action]);
        }
        //持久技能保持
        private void OnInputPerformSustain(InputAction.CallbackContext obj)
        {
            foreach (var binding in obj.action.bindings)
            {
                if (Compare(binding.name, "Move"))
                {
                    moveVector2 = obj.ReadValue<Vector2>();
                }
            }
        }
        //持久技能结束
        private void OnInputEndSustain(InputAction.CallbackContext obj)
        {
            foreach (var binding in obj.action.bindings)
            {
                if (Compare(binding.name, "Move"))
                {
                    moveVector2 = Vector2.zero;
                }
            }
            ReturnOrignalState();
        }

        private void ReturnOrignalState() => controller.Play(originalState);

        public ClipTransition TryGet(string actionName)
        {
            foreach (var key in AnimationInstant.Keys)
            {
                if (Compare(key.name,actionName))
                {
                    AnimationInstant.TryGetValue(key, out var animationClip);
                    return animationClip;
                }
            }
            Debug.unityLogger.LogError("Animation","NoFind Action Compare Animation");
            return null;
        }
        
        
    }
}
