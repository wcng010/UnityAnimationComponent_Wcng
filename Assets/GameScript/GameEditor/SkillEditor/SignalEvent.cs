using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace Wcng.SkillEditor
{
    [Serializable]
    public class SignalEvent
    {
        public SignalAsset signalAsset;
        public List<UnityEvent> events;
    }
}