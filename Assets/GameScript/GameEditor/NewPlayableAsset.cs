namespace Wcng
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [System.Serializable]
    public class NewPlayableAsset : PlayableAsset, ISerializationCallbackReceiver
    {
        [NonSerialized]
        public TimelineClip clip;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return Playable.Create(graph);
        }

        public void OnAfterDeserialize() { }

        //在序列化之前会调用该函数
        public void OnBeforeSerialize()
        {
            //获取所有TimeLineAsset
            string[] guids = AssetDatabase.FindAssets("t:TimelineAsset");
            var timelines = guids.Select(id => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(id), typeof(TimelineAsset)));

            //找到包含这个PlayableAsset的Clip
            foreach (TimelineAsset timeline in timelines)
            {
                if (timeline)
                {
                    foreach (var track in timeline.GetOutputTracks())
                    {
                        foreach (var clip in track.GetClips())
                        {
                            if (clip.asset == this)
                            {
                                this.clip = clip;

                                Debug.Log(clip.start);
                                Debug.Log(clip.end);

                                Debug.Log(clip.duration);

                                Debug.Log(clip.easeInDuration);
                                Debug.Log(clip.easeOutDuration);
                            }
                        }
                    }
                }
            }
        }
    }
}