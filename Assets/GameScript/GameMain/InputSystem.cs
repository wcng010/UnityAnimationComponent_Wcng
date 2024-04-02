using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static Wcng.FunctionLibrary;
namespace Wcng
{
    public class InputSystem: SerializedMonoBehaviour
    {
        [SerializeField]private AnimationControllerWcng animationController;
        public Dictionary<InputAction,AnimationClip> AnimationTrigger;
        
        private void action_canceled(InputAction.CallbackContext obj)
        {
            Debug.Log("ActionCanceled");
        }

        private void action_performed(InputAction.CallbackContext obj)
        {
            Debug.Log("ActionPerformed");
        }

        private void action_start(InputAction.CallbackContext obj)
        {
            Debug.Log("ActionStart");
        }
    }
        
        
}