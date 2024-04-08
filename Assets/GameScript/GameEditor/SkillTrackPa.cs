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
        public ClipTransition animancerClip;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            SkillTrackPb pb = new SkillTrackPb();
            pb.animancerClip = animancerClip;
            return ScriptPlayable<SkillTrackPb>.Create(graph, pb);
        }

        public void OnInit(ClipTransition clipTransition)
        {
            animancerClip = clipTransition;
        }
    }
}