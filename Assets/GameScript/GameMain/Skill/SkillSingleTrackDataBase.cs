using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace Wcng.SkillEditor
{
    public class SkillSingLineTrackDataBase : SkillTrackDataBase
    {

    }

    public class SkillSingLineTrackDataBase<T> : SkillSingLineTrackDataBase where T : SkillFrameEventBase
    {
        [NonSerialized, OdinSerialize]
        public Dictionary<int, T> FrameData = new Dictionary<int, T>();
    }
}
