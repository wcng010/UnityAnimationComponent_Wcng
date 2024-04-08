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
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SkillTrackMb>.Create(graph, inputCount);
        }
        
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Data/StateLoader.asset");
            var skillData = stateData.GetState(stateType);
            if (skillData.animationBegin != null)
            {
                var beginClip = CreateClip<SkillTrackPa>();
                beginClip.displayName = "begin";
                beginClip.start = skillData.animationBeginInterval;
                beginClip.duration = skillData.animationBegin.Length;
                SkillTrackPa beginPlayableAsset = beginClip.asset as SkillTrackPa;
                beginPlayableAsset?.OnInit(skillData.animationBegin);
                
                if (skillData.animationLoop != null)
                {
                    var loopClip = CreateClip<SkillTrackPa>();
                    loopClip.displayName = "loop";
                    loopClip.start = skillData.animationBeginInterval+ skillData.animationloopInterval + skillData.animationBegin.Length;
                    loopClip.duration = skillData.animationLoop.Length;
                    SkillTrackPa loopPlayableAsset = loopClip.asset as SkillTrackPa;
                    loopPlayableAsset?.OnInit(skillData.animationLoop);
                    
                    if (skillData.animationEnd != null)
                    {
                        var endClip = CreateClip<SkillTrackPa>();
                        endClip.displayName = "end";
                        endClip.start =skillData.animationBeginInterval+ skillData.animationloopInterval +skillData.animationEndInterval
                                       + skillData.animationBegin.Length + skillData.animationLoop.Length;
                        endClip.duration = skillData.animationEnd.Length;
                        SkillTrackPa endPlayableAsset = endClip.asset as SkillTrackPa;
                        endPlayableAsset?.OnInit(skillData.animationEnd);
                    }
                }
            }
            
        }
    }
}