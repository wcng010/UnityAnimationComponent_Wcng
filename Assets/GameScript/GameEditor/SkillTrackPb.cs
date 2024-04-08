using Animancer;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Wcng
{
    public class SkillTrackPb : PlayableBehaviour
    {
        public AnimancerComponent animancer;
        public ClipTransition animancerClip;
        private float _time;
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info); 
            _time = (float)playable.GetTime();
            animancer.States.Current.Time = _time;
        }
        
        public override void OnPlayableCreate(Playable playable)
        {
            base.OnPlayableCreate(playable);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (animancer.States.Current==null||animancer.States.Current.Clip != animancerClip.Clip)
            {
                animancer.Play(animancerClip);
            }
        }
        
        
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            //animancer.Stop();
            animancer.States.Current.Speed = 0;
        }
        
        /*
        public Vector3 GetScale(Playable playable)
        {
            float playProgress = (float)(playable.GetTime() / playable.GetDuration());
            return Vector3.Lerp(startScale, endScale, playProgress);
        }*/
        
#if UNITY_EDITOR
        /// <summary>[Editor-Only] Applies the starting openness value to the door in Edit Mode.</summary>
        /// <remarks>Called in Edit Mode whenever this script is loaded or a value is changed in the Inspector.</remarks>
        private void OnValidate()
        {
            AnimancerUtilities.EditModeSampleAnimation(animancerClip.Clip, animancer, _time * animancerClip.Clip.length);
        }
#endif
    }
}