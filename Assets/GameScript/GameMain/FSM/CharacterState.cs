using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Wcng.SkillEditor;

namespace Wcng.FSM
{
    [Serializable]
    public struct From
    {
        public double start;
        public double interval;
        public From(double s, double i)
        {
            start = s;
            interval = i;
        }
    }

    [Serializable]
    public abstract class CharacterState : SerializedScriptableObject,IState
    {
        protected PlayableDirector Director;
        protected AnimancerComponent Controller;
        protected CharacterStateMachine StateMachine;
        protected InputComponent InputComponent;
        public TimelineAsset timelineAsset;
        [FoldoutGroup("动画")] public Dictionary<ClipTransition, From> AnimationClips;
        [FoldoutGroup("音频")] public Dictionary<AudioClip, From> AudioClips;
        [FoldoutGroup("特效")] public Dictionary<GameObject, From> EffectClips;
        //[FoldoutGroup("事件")] public Dictionary<SignalAsset, double> EventSignals;
        [FoldoutGroup("事件")] public Dictionary<SignalEvent, double> MyEventSignals;
        [field:SerializeField] public  bool IsLoop { get; set; }
        [field:SerializeField] public bool CanChangeState { get; set; }
        
        public void OnInit<TState>(StateMachine<TState> stateMachine,AnimancerComponent controller, InputComponent inputComponent, PlayableDirector playableDirector) where TState : class, IState
        {
            Controller = controller;
            StateMachine = stateMachine as CharacterStateMachine;
            Director = playableDirector;
            InputComponent = inputComponent;
        }
        
        public void WriteEventSignals(SignalAsset signal,double pos)
        {
            SignalEvent temp = null;
            if (signal != null)
            {
                foreach (var eventSignal in MyEventSignals)
                {
                    if (signal == eventSignal.Key.signalAsset)
                    {
                        temp = eventSignal.Key;
                        break;
                    }
                }
            }
            if (temp != null) MyEventSignals[temp] = pos;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        
        
        public void WriteAnimationClip(AnimationClip animationClip,double start,double interval)
        {
            ClipTransition temp = null;
            foreach (var clip in AnimationClips)
            {
                if (clip.Key.Clip == animationClip)
                {
                    temp = clip.Key;
                    break;
                }
            }
            From from = new From(start,interval);
            if (temp != null) AnimationClips[temp] = from;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        
        public void WriteAudioClip(AudioClip audioClip,double start,double interval)
        {
            From from = new From(start,interval);
            if (audioClip != null) AudioClips[audioClip] = from;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        
        public void WriteEffectAudio(GameObject effectObj,double start,double interval)
        {
            From from = new From(start,interval);
            if (effectObj != null) EffectClips[effectObj] = from;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
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
