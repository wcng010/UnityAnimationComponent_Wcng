using System;
using Animancer;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Wcng
{
    [Serializable]
    public class SkillTrackPa : PlayableAsset
    {
        public AnimancerComponent Animancer =>
            GameObject.FindWithTag("Player").GetComponentInChildren<AnimancerComponent>();
        public ClipTransition animancerClip;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            SkillTrackPb testDemo1 = new SkillTrackPb();
            testDemo1.animancer = Animancer;
            testDemo1.animancerClip = animancerClip;
            return ScriptPlayable<SkillTrackPb>.Create(graph, testDemo1);
        }

        public void OnInit(AnimancerComponent animancerComponent,ClipTransition clipTransition)
        {
            animancerClip = clipTransition;
        }
    }
}