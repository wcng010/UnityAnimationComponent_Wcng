using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

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

        protected PlayableDirector Director;
        protected AnimancerComponent Controller;
        protected CharacterStateMachine StateMachine;
        protected InputComponent InputComponent;
        public TimelineAsset timelineAsset;

        [FoldoutGroup("动画")]
        public float animationBeginInterval;
        [FoldoutGroup("动画")]
        public float animationloopInterval;
        [FoldoutGroup("动画")]
        public float animationEndInterval;
        [FoldoutGroup("动画")]
        public ClipTransition animationBegin;
        [FoldoutGroup("动画")]
        public ClipTransition animationLoop;
        [FoldoutGroup("动画")]
        public ClipTransition animationEnd;

        [FoldoutGroup("音频")] 
        public float audiobeginInterval;
        [FoldoutGroup("音频")]
        public float audioloopInterval;
        [FoldoutGroup("音频")]
        public float audioEndInterval;
        [FoldoutGroup("音频")]
        public AudioClip audioClipBegin;
        [FoldoutGroup("音频")]
        public AudioClip audioClipLoop;
        [FoldoutGroup("音频")]
        public AudioClip audioClipEnd;
        
        [FoldoutGroup("特效")]
        public float effectBeginInterval;
        [FoldoutGroup("特效")]
        public float effectloopInterval;
        [FoldoutGroup("特效")]
        public float effectEndInterval;
        [FoldoutGroup("特效")]
        public GameObject effectBegin;
        [FoldoutGroup("特效")]
        public GameObject effectLoop;
        [FoldoutGroup("特效")]
        public GameObject effectEnd;
        
        [field:SerializeField] public  bool IsLoop { get; set; }
        [field:SerializeField] public bool CanChangeState { get; set; }
        
        public void OnInit<TState>(StateMachine<TState> stateMachine,AnimancerComponent controller, InputComponent inputComponent, PlayableDirector playableDirector) where TState : class, IState
        {
            Controller = controller;
            StateMachine = stateMachine as CharacterStateMachine;
            Director = playableDirector;
            InputComponent = inputComponent;
        }
        public virtual void OnEnter()
        {
            Director.Play(timelineAsset);
            if (IsLoop) Director.extrapolationMode = DirectorWrapMode.Loop;
            else Director.extrapolationMode = DirectorWrapMode.None;
        }

        public abstract void OnPhysicUpdate();

        public abstract void OnLogicUpdate();

        public virtual void OnExit()
        {
            Director.Stop();
        }


        protected IEnumerator AnimationPlay()
        {
            Controller.Play(animationBegin);
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
