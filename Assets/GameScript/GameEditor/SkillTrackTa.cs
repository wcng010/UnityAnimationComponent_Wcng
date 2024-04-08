using System;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace Wcng
{
    [TrackColor(0, 0, 0)]
    [TrackClipType(typeof(SkillTrackPa))]
    public class SkillTrackTa : TrackAsset
    {
        
        public StateLoaderSo stateData;
        private bool _isFirstEnable = false;
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SkillTrackMb>.Create(graph, inputCount);
        }

        public void OnCreateTrack(Type stateType)
        {
            stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Data/StateLoader.asset");
            var skillData = stateData.GetState(stateType);
            if (skillData.AnimationBegin != null)
            {
                var beginClip = CreateClip<SkillTrackPa>();
                beginClip.displayName = "begin";
                beginClip.start = skillData.animationBeginInterval;
                beginClip.duration = skillData.AnimationBegin.Length;
                SkillTrackPa beginPlayableAsset = beginClip.asset as SkillTrackPa;
                beginPlayableAsset?.OnInit(skillData.Controller, skillData.AnimationBegin);
                
                if (skillData.AnimationLoop != null)
                {
                    var loopClip = CreateClip<SkillTrackPa>();
                    loopClip.displayName = "loop";
                    loopClip.start = skillData.animationBeginInterval+ skillData.animationloopInterval + skillData.AnimationBegin.Length;
                    loopClip.duration = skillData.AnimationLoop.Length;
                    SkillTrackPa loopPlayableAsset = loopClip.asset as SkillTrackPa;
                    loopPlayableAsset?.OnInit(skillData.Controller, skillData.AnimationLoop);
                    
                    if (skillData.AnimationEnd != null)
                    {
                        var endClip = CreateClip<SkillTrackPa>();
                        endClip.displayName = "end";
                        endClip.start =skillData.animationBeginInterval+ skillData.animationloopInterval +skillData.animationEndInterval
                                       + skillData.AnimationBegin.Length + skillData.AnimationLoop.Length;
                        endClip.duration = skillData.AnimationEnd.Length;
                        SkillTrackPa endPlayableAsset = endClip.asset as SkillTrackPa;
                        endPlayableAsset?.OnInit(skillData.Controller, skillData.AnimationEnd);
                    }
                }
            }
        }
    }
}