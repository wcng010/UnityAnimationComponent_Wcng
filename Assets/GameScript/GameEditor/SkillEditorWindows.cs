using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine.Timeline;
using Wcng;

namespace Wcng
{
    public  class SkillEditorWindows : OdinEditorWindow
    {
        private static List<TimelineAsset> _TimelineAssets = new List<TimelineAsset>();
        private static StateLoaderSo _Setting;
        [MenuItem("Tools/LoadSkillTimeLine")]
        private static void Load()
        {
            for (int i = 0; i < _TimelineAssets.Count; i++)
            {
                //Animation Part
                var animGroup = _TimelineAssets[i].CreateTrack<GroupTrack>(); 
                animGroup.name = "动作";
                var skillTrack = _TimelineAssets[i].CreateTrack<SkillTrackTa>();
                skillTrack.SetGroup(animGroup);
                skillTrack.OnCreateTrack(_Setting.states[i].GetType());
                
                //Audio Part
                var audioGroup = _TimelineAssets[i].CreateTrack<GroupTrack>();
                audioGroup.name = "音频";
                var audioTrack = _TimelineAssets[i].CreateTrack<AudioExtensionTrack>();
                audioTrack.OnCreateTrack(_Setting.states[i].GetType());
                audioTrack.SetGroup(audioGroup);
                
                //Effect Part
                var effectGroup = _TimelineAssets[i].CreateTrack<GroupTrack>();
                effectGroup.name = "特效";
                var effectTrack = _TimelineAssets[i].CreateTrack<EffectTrack>();
                effectTrack.OnCreateTrack(_Setting.states[i].GetType());
                effectTrack.SetGroup(effectGroup);
                _Setting.states[i].timelineAsset = _TimelineAssets[i];
            }
        }

        [MenuItem("Tools/CreateSkillTimeLine")]
        private static void Create()
        {
            _TimelineAssets.Clear();
            _Setting = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Data/StateLoader.asset");
            foreach (var state in _Setting.states)
            {
                var timelineAsset = CreateInstance<TimelineAsset>();
                _TimelineAssets.Add(timelineAsset);
                AssetDatabase.CreateAsset(timelineAsset, "Assets/Art/Timeline/" + state.name + ".playable");
                AssetDatabase.Refresh();
            }
        }
    }
}
