using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Wcng
{
    public class EffectTrack:ControlTrack
    {
        private GameObject SourceObj =>  GameObject.FindWithTag("PlayerEffect");
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Data/StateLoader.asset");
            var effectData = stateData.GetState(stateType);
            if (effectData.effectBegin != null)
            {
                var beginClip = CreateClip<ControlPlayableAsset>();
                beginClip.displayName = "effect1";
                beginClip.start = effectData.effectBeginInterval;
                beginClip.duration = 1f;
                var beginPlayableAsset = beginClip.asset as ControlPlayableAsset;
                if (beginPlayableAsset != null)
                {
                    beginPlayableAsset.sourceGameObject.defaultValue= SourceObj;
                    beginPlayableAsset.prefabGameObject = effectData.effectBegin;
                }
            }
            if (effectData.effectLoop != null)
            {
                var loopClip = CreateClip<ControlPlayableAsset>();
                loopClip.displayName = "effect2";
                loopClip.start = effectData.effectloopInterval;
                loopClip.duration = 1f;
                var loopPlayableAsset =  loopClip.asset as ControlPlayableAsset;
                if (loopPlayableAsset != null)
                {
                    loopPlayableAsset.sourceGameObject.defaultValue= SourceObj;
                    loopPlayableAsset.prefabGameObject = effectData.effectLoop;
                }
                    
            }
            if (effectData.effectEnd != null)
            {
                var endClip = CreateClip<ControlPlayableAsset>();
                endClip.displayName = "effect3";
                endClip.start = effectData.effectEndInterval;
                endClip.duration = 1f;
                var endPlayableAsset = endClip.asset as ControlPlayableAsset;
                if (endPlayableAsset != null)
                {
                    endPlayableAsset.sourceGameObject.defaultValue= SourceObj;
                    endPlayableAsset.prefabGameObject = effectData.effectLoop;
                }
            }
        }
    }
}