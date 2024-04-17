using System;
using UnityEditor;
using UnityEngine.Timeline;
using Wcng.FSM;

namespace Wcng.SkillEditor
{
    public class EventExtensionTrack: SignalTrack
    {
        public void OnCreateTrack(Type stateType)
        {
            var stateData = AssetDatabase.LoadAssetAtPath<StateLoaderSo>("Assets/Skill/Data/StateLoader.asset");
            var eventData = stateData.GetState(stateType);
            if (eventData.MyEventSignals != null)
            {
                foreach (var signal in eventData.MyEventSignals)
                {
                    var temp = CreateMarker<SignalEmitter>(signal.Value);
                    temp.asset = signal.Key.signalAsset;
                }
            }
        }
    }
}