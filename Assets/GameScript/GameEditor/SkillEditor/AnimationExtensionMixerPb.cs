using UnityEngine;
using UnityEngine.Playables;

namespace Wcng.SkillEditor
{
    public class AnimationExtensionMixerPb: PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();
            Vector3 blendScale = Vector3.zero;
            for (int i = 0; i < inputCount; i++)
            {
                var tempPlay = playable.GetInput(i);
                ScriptPlayable<AnimationExtensionPb> tempPlayable = (ScriptPlayable<AnimationExtensionPb>)tempPlay;
                var mixBehaviour = tempPlayable.GetBehaviour();
                float weight = playable.GetInputWeight(i);
               // blendScale += weight * mixBehaviour.GetScale(tempPlayable);
            }
        }
    }
}