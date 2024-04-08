using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Wcng
{
    [Serializable]
    public abstract class CharacterState : ScriptableObject,IState
    {
        public enum CharacterStateType
        {
            IdleState,
            MoveState,
            Combo1State,
            DefenseState
        }

        [field:SerializeField]public AnimancerComponent Controller { get; private set; }
        protected CharacterStateMachine StateMachine;
        protected InputComponent InputComponent;
        public float animationBeginInterval;
        public float animationloopInterval;
        public float animationEndInterval;
        public ClipTransition AnimationBegin;
        public ClipTransition AnimationLoop;
        public ClipTransition AnimationEnd;

        public abstract bool CanChangeState { get; }


        public void OnInit<TState>(AnimancerComponent controller, StateMachine<TState> stateMachine, InputComponent inputComponent) where TState : class, IState
        {
            this.Controller = controller;
            StateMachine = stateMachine as CharacterStateMachine;
            InputComponent = inputComponent;
        }
        public virtual void OnEnter(){
            if (AnimationBegin != null) Controller.Play(AnimationBegin);
            else if (AnimationLoop != null) Controller.Play(AnimationLoop);
            else if (AnimationEnd != null) Controller.Play(AnimationEnd);
        }

        public abstract void OnPhysicUpdate();

        public abstract void OnLogicUpdate();

        public abstract void OnExit();


        protected IEnumerator AnimationPlay()
        {
            Controller.Play(AnimationBegin);
            yield return null;
        }
        
        protected IEnumerator WaitForInvoke(Action action,float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            action.Invoke();
        }
        protected IEnumerator WaitForInvoke(Action<int> action,float waitTime,int count)
        {
            yield return new WaitForSeconds(waitTime);
            action.Invoke(count);
        }
    }
}
