using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Wcng.FSM;

namespace Wcng.SkillEditor
{
    [TrackColor(0, 0, 0)]
    [TrackClipType(typeof(AnimationExtensionPa))]
    public class AnimationExtensionTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<AnimationExtensionMixerPb>.Create(graph, inputCount);
        }
        
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Skill/Data/StateLoader.asset");
            var skillData = stateData.GetState(stateType);
            int index = 0;
            double interval = 0;
            if (skillData.AnimationClips != null)
            {
                foreach (var clip in skillData.AnimationClips)
                {
                    var displayClip = CreateClip<AnimationExtensionPa>();
                    displayClip.displayName = "animation_" + index++;
                    if (clip.Value.interval != 0)
                    {
                        displayClip.start = clip.Value.start;
                        displayClip.duration = clip.Value.interval;
                    }
                    else
                    {
                        displayClip.start = interval;
                        displayClip.duration = clip.Key.Length;
                        interval += displayClip.duration;
                    }
                    AnimationExtensionPa beginPlayableAsset = displayClip.asset as AnimationExtensionPa;
                    beginPlayableAsset?.OnInit(clip.Key);
                }
            }
        }
    }
}