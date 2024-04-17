using System;
using UnityEditor;
using UnityEngine.Timeline;
using Wcng.FSM;

namespace Wcng.SkillEditor
{
    [TrackColor(128, 128, 0)]
    [TrackClipType(typeof(EffectExtensionPa))]
    public class EffectExtensionTrack:ControlTrack
    {
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Skill/Data/StateLoader.asset");
            var effectData = stateData.GetState(stateType);
            
            int index = 0;
            if (effectData.EffectClips != null)
            {
                foreach (var clip in effectData.EffectClips)
                {
                    var displayClip = CreateClip<EffectExtensionPa>();
                    displayClip.displayName = "effect_" + index++;
                    displayClip.start = clip.Value.start;
                    if (clip.Value.interval != 0)
                    {
                        displayClip.duration = clip.Value.interval;
                    }
                    else
                    {
                        displayClip.duration = 1;
                    }
                    EffectExtensionPa beginPlayableAsset = displayClip.asset as EffectExtensionPa;
                    if (beginPlayableAsset != null)
                    {
                        //beginPlayableAsset.prefabGameObject = clip.Key;
                        beginPlayableAsset.prefabGameObject = clip.Key;
                    }
                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}