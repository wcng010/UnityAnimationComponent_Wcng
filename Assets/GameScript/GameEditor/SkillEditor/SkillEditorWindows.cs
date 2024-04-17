using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine.Timeline;
using Wcng.FSM;


namespace Wcng.SkillEditor
{
    public  class SkillEditorWindows : OdinEditorWindow
    {
        private static StateLoaderSo Setting => _Setting? _Setting : _Setting =  AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Skill/Data/StateLoader.asset");
        private static StateLoaderSo _Setting;
        [MenuItem("Tools/SkillTimeline/LoadSkillTimeLine")]
        private static void Load()
        {
            List<TimelineAsset> timelineAssets = new List<TimelineAsset>(); 
            //清除Track
            for (int i = 0; i < Setting.states.Count; ++i)
            {
                var timelineAsset = AssetDatabase.LoadAssetAtPath<TimelineAsset>("Assets/Skill/Timeline/" + Setting.states[i].name + ".playable");
                timelineAssets.Add(timelineAsset);
                foreach (var track in timelineAsset.GetRootTracks())
                {
                    timelineAsset.DeleteTrack(track);
                }
            }
            
            for (int i = 0; i < timelineAssets.Count; i++)
            {

                //Animation Part
                var animGroup = timelineAssets[i].CreateTrack<GroupTrack>(); 
                animGroup.name = "动作";
                var skillTrack = timelineAssets[i].CreateTrack<AnimationExtensionTrack>();
                skillTrack.SetGroup(animGroup);
                skillTrack.OnCreateTrack(Setting.states[i].GetType());
                
                //Audio Part
                var audioGroup = timelineAssets[i].CreateTrack<GroupTrack>();
                audioGroup.name = "音频";
                var audioTrack = timelineAssets[i].CreateTrack<AudioExtensionTrack>();
                audioTrack.OnCreateTrack(Setting.states[i].GetType());
                audioTrack.SetGroup(audioGroup);
                
                //Effect Part
                var effectGroup = timelineAssets[i].CreateTrack<GroupTrack>();
                effectGroup.name = "特效";
                var effectTrack = timelineAssets[i].CreateTrack<EffectExtensionTrack>();
                effectTrack.OnCreateTrack(Setting.states[i].GetType());
                effectTrack.SetGroup(effectGroup);
                
                //Event Part
                timelineAssets[i].CreateMarkerTrack();
                var eventData = Setting.states[i];
                if (eventData.MyEventSignals != null)
                {
                    foreach (var signal in eventData.MyEventSignals)
                    {
                        var emitter = timelineAssets[i].markerTrack.CreateMarker<SignalEmitter>(signal.Value);
                        emitter.asset = signal.Key.signalAsset;
                    }
                }
                Setting.states[i].timelineAsset = timelineAssets[i];
            }
        }

        [MenuItem("Tools/SkillTimeline/CreateSkillTimeLine")]
        private static void Create()
        {
            foreach (var state in Setting.states)
            {
                var timelineAsset = CreateInstance<TimelineAsset>();
                AssetDatabase.CreateAsset(timelineAsset, "Assets/Skill/Timeline/" + state.name + ".playable");
                AssetDatabase.Refresh();
            }
        }
        [MenuItem("Tools/SkillTimeline/SaveSkillTimeLine")]
        private static void Write()
        {
            foreach (var state in Setting.states)
            {
                var timeline =
                    AssetDatabase.LoadAssetAtPath<TimelineAsset>("Assets/Skill/Timeline/" + state.name + ".playable");
                //事件层
                foreach (var marker in timeline.GetRootTrack(0).GetMarkers())
                {
                    var temp = marker as SignalEmitter;
                    if (temp != null) 
                        state.WriteEventSignals(temp.asset, temp.time);
                }
                //动作层
                foreach (var track in timeline.GetRootTrack(1).GetChildTracks())
                {
                    foreach (var clip in track.GetClips())
                    {
                        AnimationExtensionPa animationTrack = clip.asset as AnimationExtensionPa;
                        state.WriteAnimationClip(animationTrack.animancerClip.Clip,clip.start,clip.duration);
                    }
                }
                //音频层
                foreach (var track in timeline.GetRootTrack(2).GetChildTracks())
                {
                    foreach (var clip in track.GetClips())
                    {
                        AudioPlayableAsset audioTrack = clip.asset as AudioPlayableAsset;
                        if (audioTrack != null) 
                            state.WriteAudioClip(audioTrack.clip, clip.start, clip.duration);
                    }
                }
                //特效层
                foreach (var track in timeline.GetRootTrack(3).GetChildTracks())
                {
                    foreach (var clip in track.GetClips())
                    {
                        ControlPlayableAsset effectTrack = clip.asset as ControlPlayableAsset;
                        if (effectTrack != null) 
                            state.WriteEffectAudio(effectTrack.prefabGameObject, clip.start, clip.duration);
                    }
                }

            }
        }
    }
}
