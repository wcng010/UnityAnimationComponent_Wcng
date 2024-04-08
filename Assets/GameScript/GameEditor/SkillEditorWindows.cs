using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Timeline;
using Wcng;

public class SkillEditorWindows : OdinEditorWindow
{
    private static List<TimelineAsset> _TimelineAssets = new List<TimelineAsset>();
    private static StateLoaderSo _Setting;
    [MenuItem("Tools/LoadSkillTimeLine")]
    private static void Load()
    {
        for (int i = 0; i < _TimelineAssets.Count; i++)
        {
            var animGroup = _TimelineAssets[i].CreateTrack<GroupTrack>(); 
            animGroup.name = "动作";
            var skillTrack = _TimelineAssets[i].CreateTrack<SkillTrackTa>();
            skillTrack.SetGroup(animGroup);
            skillTrack.OnCreateTrack(_Setting.states[i].GetType());
            var videoGroup = _TimelineAssets[i].CreateTrack<GroupTrack>();
            videoGroup.name = "音频";
            var effectGroup = _TimelineAssets[i].CreateTrack<GroupTrack>();
            effectGroup.name = "特效";
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
