using System;
using UnityEditor;
using UnityEngine.Timeline;
using Wcng.FSM;

namespace Wcng.SkillEditor
{
    public class AudioExtensionTrack : AudioTrack
    { 
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Skill/Data/StateLoader.asset");
            var audiodData = stateData.GetState(stateType);
            int index = 0;
            double interval = 0;
            if (audiodData.AnimationClips != null)
            {
                foreach (var clip in audiodData.AudioClips)
                {
                    var displayClip = CreateClip<AudioPlayableAsset>();
                    displayClip.displayName = "audio_" + index++;
                    if (clip.Value.interval != 0)
                    {
                        displayClip.start = clip.Value.start;
                        displayClip.duration = clip.Value.interval;
                    }
                    else
                    {
                        displayClip.start = interval;
                        displayClip.duration = clip.Key.length;
                        interval += displayClip.duration;
                    }
                    AudioPlayableAsset beginPlayableAsset = displayClip.asset as AudioPlayableAsset;
                    if (beginPlayableAsset != null)
                        beginPlayableAsset.clip = clip.Key;
                }
            }
        }
    }
}