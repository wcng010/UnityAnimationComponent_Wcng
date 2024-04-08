using UnityEngine;
using UnityEngine.Playables;

namespace Wcng
{
    public class SkillTrackMb: PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();
            Vector3 blendScale = Vector3.zero;
            for (int i = 0; i < inputCount; i++)
            {
                var tempPlay = playable.GetInput(i);
                ScriptPlayable<SkillTrackPb> tempPlayable = (ScriptPlayable<SkillTrackPb>)tempPlay;
                var mixBehaviour = tempPlayable.GetBehaviour();
                float weight = playable.GetInputWeight(i);
               // blendScale += weight * mixBehaviour.GetScale(tempPlayable);
            }
        }
    }
}