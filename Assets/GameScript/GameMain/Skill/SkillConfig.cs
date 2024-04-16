using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace Wcng.SkillEditor
{
    [CreateAssetMenu(fileName = "SkillConfig",menuName = "Data/SkillConfig")]
    public class SkillConfig : SerializedScriptableObject
    {
        [LabelText("技能名称")] public string skillName;
        [LabelText("帧数上线")] public int FrameCount = 100;
        [LabelText("帧率")] public int FrameRate = 30;
        
        [NonSerialized, OdinSerialize]
        public Dictionary<string, SkillTrackDataBase> trackDataDic = new Dictionary<string, SkillTrackDataBase>();

        public void AddTrack(string trackType, SkillTrackDataBase dataBase)
        {
            trackDataDic.Add(trackType, dataBase);
        }

        public void RemoveTrack(string trackType)
        {
            trackDataDic.Remove(trackType);
        }

        public SkillTrackDataBase GetTrack(string trackType)
        {
            return trackDataDic[trackType];
        }

#if UNITY_EDITOR

        //[Button]
        //public void PlaceEffecetASExcaple()
        //{
        //    int i = 0;
        //    foreach (SkillEffectEvent item in (trackDataDic["ARPG_AE_JOKER.SkillEditor.EffectTrack"] as SkillMultiLineTrackDataBase<SkillEffectEvent>).FrameData)
        //    {
        //        item.FrameIndex = i;
        //        item.SetFrameDuration(30);
        //        i += 30;
        //    }
        //}
        //外部事件订阅
        public static Action skillConfigOnValidate;

        public static void SetSkillValidateAction(Action action)
        { skillConfigOnValidate = action; }

        private void OnValidate()
        {
            skillConfigOnValidate?.Invoke();
        }
#endif
    }
}
