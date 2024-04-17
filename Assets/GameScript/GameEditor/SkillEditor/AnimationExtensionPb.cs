using Animancer;
using Animancer.Editor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Wcng.SkillEditor
{
    public class AnimationExtensionPb : PlayableBehaviour
    {
        public AnimancerComponent animancer => _animancer? _animancer: _animancer = GameObject.FindWithTag("Player").GetComponentInChildren<AnimancerComponent>();
        private AnimancerComponent _animancer;
        public ClipTransition animancerClip;
        private float _time;
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (Application.isEditor)
            {
                base.PrepareFrame(playable, info);
                animancer.States.Current.Speed = 0;
                _time = (float)playable.GetTime();
                animancer.States.Current.Time = _time;
                OnValidate();
            }
        }
        
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (animancer.States.Current==null||animancer.States.Current.Clip != animancerClip.Clip)
            {
                animancer.Play(animancerClip);
            }
        }
        
        private void OnValidate()
        {
            AnimancerUtilities.EditModeSampleAnimation(animancerClip.Clip, animancer, _time * animancerClip.Clip.length);
        }

    }
}