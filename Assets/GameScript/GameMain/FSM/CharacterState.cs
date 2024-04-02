using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Wcng
{
    [Serializable]
    public abstract class CharacterState : IState
    {
        public enum CharacterStateType
        {
            IdleState,
            MoveState,
            Combo1State,
            DefenseState
        }

        protected AnimancerComponent Controller;
        protected CharacterStateMachine StateMachine;
        protected InputComponent InputComponent;
        public ClipTransition AnimationBegin;
        public ClipTransition AnimationLoop;
        public ClipTransition AnimationEnd;

        public abstract bool CanChangeState { get; }


        public void OnInit<TState>(AnimancerComponent controller, StateMachine<TState> stateMachine, InputComponent inputComponent) where TState : class, IState
        {
            Controller = controller;
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
