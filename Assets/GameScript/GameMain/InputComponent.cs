using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Wcng
{
    public enum InputKey
    {
        KeyQ,
        KeyW,
        KeyE,
        KeyR,
        KeyT,
        KeyY,
        KeyU,
        KeyI,
        KeyO,
        KeyP,
        KeyA,
        KeyS,
        KeyD,
        KeyF,
        KeyG,
        KeyH,
        KeyJ,
        KeyK,
        KeyL,
        KeyZ,
        KeyX,
        KeyC,
        KeyV,
        KeyB,
        KeyN,
        KeyM,
        KeySpace,
        KeyMove,
        MouseClickLeft,
        MouseClickRight,
        KeyShift
    }

    public class InputComponent : SerializedMonoBehaviour
    {
        [Title("角色移动方向")]
        public Vector2 moveVec2;
        public Dictionary<InputAction,InputKey> InputActions;
        [NonSerialized]
        public Dictionary<InputKey, bool> InputBools;
        [NonSerialized]
        public List<InputKey> InputKeys;

        private void Awake()
        {
            InputBools = new Dictionary<InputKey, bool>();
            foreach (var ation in InputActions)
            {
                InputBools.Add(ation.Value,false);
            }
            InputKeys = new List<InputKey>(InputBools.Keys);
            
            foreach (var key in InputActions.Keys)
            {
                Debug.Log(key);
                key.Enable();
                key.started += OnPressedStart;
                key.canceled += OnPressedEnd;
            }
        }

        private void OnPressedStart(InputAction.CallbackContext obj)
        {
            for (int i = 0; i < InputKeys.Count; ++i)
            {
                if (InputKeys[i] == InputActions[obj.action])
                {
                    InputBools[InputKeys[i]] = true;
                }
            }

            if (obj.action.bindings[0].name == "Move")
            {
                moveVec2 = obj.ReadValue<Vector2>();
            }
        }

        private void OnPressedEnd(InputAction.CallbackContext obj)
        {
            for (int i = 0; i < InputKeys.Count; ++i)
            {
                if (InputKeys[i] == InputActions[obj.action])
                {
                    InputBools[InputKeys[i]] = false;
                }
            }
        }

        public List<InputKey> GetPressedKeys()
        {
            List<InputKey> keys = new List<InputKey>();
            foreach (var inputBool in InputBools)
            {
                if (inputBool.Value)
                {
                    keys.Add(inputBool.Key);
                }
            }
            return keys;
        }
    }
}
