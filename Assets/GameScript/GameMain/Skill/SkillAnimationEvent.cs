using Animancer;
using UnityEngine;
using Wcng.SkillEditor;

namespace Wcng
{
    public class SkillAnimationEvent : SkillFrameEventBase
    {
        public AnimancerComponent AnimancerComponent;
        public ClipTransition Animation;     //clip
        public bool ApplyRootMotion;              //clip
        public float TransitionTime = 0.25f;    //过渡时间
#if UNITY_EDITOR
        public int DurationFrame;               //动画时间
#endif

        public override string GetName()
        {
            return Animation.Clip.name;
        }

        public override object GetObject()
        {
            return Animation.Clip;
        }

        public override void SetObject(object value)
        {
            Animation.Clip = value as AnimationClip;
        }

        public override int GetFrameDuration(int frameRate)
        {
            return DurationFrame;
        }

        public override void SetFrameDuration(int value)
        {
            DurationFrame = value;
        }

        public override void SetName(string value)
        {
        }

        public void Play()
        {
            if (AnimancerComponent.States.Count == 0 || !AnimancerComponent.IsPlaying() || AnimancerComponent.States.Current.Clip != Animation.Clip)
            {
                AnimancerComponent.Play(Animation);
            }
        }

        public void Stop()
        {
            if (AnimancerComponent.IsPlaying()) ;
            AnimancerComponent.Stop(Animation);
        }
    }
}
