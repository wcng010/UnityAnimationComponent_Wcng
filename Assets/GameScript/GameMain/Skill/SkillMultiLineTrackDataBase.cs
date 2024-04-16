using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using Wcng.SkillEditor;

namespace GameScript.GameMain.Skill
{
    public class SkillMultiLineTrackDataBase : SkillTrackDataBase
    {
    }

    /// <summary>
    /// 轨道数据
    /// </summary>
    [Serializable]
    public class SkillMultiLineTrackDataBase<T> : SkillMultiLineTrackDataBase where T : SkillFrameEventBase
    {
        [NonSerialized, OdinSerialize]
        public List<T> FrameData = new List<T>();
    }
}