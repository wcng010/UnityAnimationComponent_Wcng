using System;
using UnityEngine;
using UnityEngine.Serialization;
using Wcng;
using static Wcng.FunctionLibrary;
namespace Wcng
{
    public class MoveComponent : MonoBehaviour
    {
        private Vector3 _MoveVector;
        private float _CurrentSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private InputComponent inputComponent;
        [SerializeField] private StateComponent stateComponent;
        [SerializeField] private CharacterController controller;
        void Update()
        {
            var moveForwardVector3 = ConvertMoveInputToCameraSpace(Camera.main.transform, inputComponent.moveVec2.x, inputComponent.moveVec2.y);
            MovePlayer(moveForwardVector3.x,moveForwardVector3.z);
        }
    
        public void MovePlayer(float moveX, float moveZ)
        {
            float speed = 0;
            Type type = stateComponent.GetCurrentState().GetType();
            if (type == typeof(MoveState)) speed = moveSpeed;
            else if (type == typeof(RunState)) speed = moveSpeed * 2;
            
            if ((type == typeof(MoveState)||type == typeof(RunState)) &&inputComponent.moveVec2 != Vector2.zero)
            {
                // Update the move Deltas
                _MoveVector.x = moveX;
                _MoveVector.z = moveZ;
                _MoveVector.Normalize();

                // gradually move toward the desired speed
                // _currentSpeed = Mathf.MoveTowards(_currentSpeed, moveSpeed, acceleration * Time.deltaTime);
            
                // Move the character
                _MoveVector *= speed*0.001f;
                controller.Move(_MoveVector);
                float step = rotateSpeed * Time.deltaTime;
                if (_MoveVector != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(_MoveVector, Vector3.up);
                    transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, step);
                }
            }
            else
            {
                controller.velocity.Set(0,controller.velocity.y,0);
            }
        }
    }
}
 