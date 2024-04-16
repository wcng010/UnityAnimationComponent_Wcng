using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Wcng;

public class TimelineTest : MonoBehaviour
{
    public TimelineAsset timelineAsset;
    private GroupTrack dongzuoGroup;
    // Start is called before the first frame update
    void Start()
    {
        dongzuoGroup = timelineAsset.CreateTrack<GroupTrack>();
        dongzuoGroup.name = "动作";
        //var playable = timelineAsset.CreateTrack<SkillTrackTa>();
        //timelineAsset.Crea
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dongzuoGroup.outputs);
    }
}
