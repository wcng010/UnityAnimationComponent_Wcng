using System;
using UnityEditor;
using UnityEngine.Timeline;

namespace Wcng
{
    public class AudioExtensionTrack : AudioTrack
    { 
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Data/StateLoader.asset");
            var audiodData = stateData.GetState(stateType);
            if (audiodData.audioClipBegin != null)
            {
                var beginClip = CreateClip<AudioPlayableAsset>();
                beginClip.displayName = "audio1";
                beginClip.start = audiodData.audiobeginInterval;
                beginClip.duration = 1f;
                var beginPlayableAsset = beginClip.asset as AudioPlayableAsset;
                if (beginPlayableAsset != null)
                {
                    //beginPlayableAsset.sourceGameObject.defaultValue= SourceObj;
                    beginPlayableAsset.clip = audiodData.audioClipBegin;
                }
            }
            if (audiodData.audioClipLoop != null)
            {
                var loopClip = CreateClip<AudioPlayableAsset>();
                loopClip.displayName = "audio2";
                loopClip.start = audiodData.effectloopInterval;
                loopClip.duration = 1f;
                var loopPlayableAsset =  loopClip.asset as AudioPlayableAsset;
                if (loopPlayableAsset != null)
                {
                    //loopPlayableAsset.sourceGameObject.defaultValue= SourceObj;
                    loopPlayableAsset.clip = audiodData.audioClipLoop;
                }
            }
            if (audiodData.audioClipEnd != null)
            {
                var endClip = CreateClip<AudioPlayableAsset>();
                endClip.displayName = "audio3";
                endClip.start = audiodData.effectEndInterval;
                endClip.duration = 1f;
                var endPlayableAsset = endClip.asset as AudioPlayableAsset;
                if (endPlayableAsset != null)
                {
                    //endPlayableAsset.sourceGameObject.defaultValue= SourceObj;
                    endPlayableAsset.clip = audiodData.audioClipEnd;
                }
            }
        }
    }
}