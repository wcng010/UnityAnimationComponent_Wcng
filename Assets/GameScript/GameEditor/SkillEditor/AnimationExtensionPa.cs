using System;
using Animancer;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Wcng.SkillEditor
{
    [Serializable]
    public class AnimationExtensionPa : PlayableAsset
    {
        public ClipTransition animancerClip;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            AnimationExtensionPb pb = new AnimationExtensionPb();
            pb.animancerClip = animancerClip;
            return ScriptPlayable<AnimationExtensionPb>.Create(graph, pb);
        }

        public void OnInit(ClipTransition clipTransition)
        {
            animancerClip = clipTransition;
        }
    }
}