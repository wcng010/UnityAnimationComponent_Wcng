using System;
using UnityEngine;
using UnityEngine.Playables;

using Object = UnityEngine.Object;

namespace Wcng.SkillEditor
{
    [Serializable]
    public class EffectExtensionPb : PlayableBehaviour
    {
        public GameObject Parent => parnet? parnet:parnet = GameObject.FindWithTag("PlayerEffect");
        [SerializeField] private GameObject parnet;
        [SerializeField] private GameObject prefabEffect;
        private EffectExtensionPa _pa;
        private ParticleSystem CurrentEffect
        {
            get
            {
                return currentEffect;
            }
            set
            {
                //赋值时，如果上一个Effect仍存在则销毁
                if (currentEffect)
                {
                    Object.Destroy(currentEffect);
                }
                currentEffect = value;
            }
        }

        [SerializeField] private ParticleSystem currentEffect;

        private float _time = 0;
        
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            //_pa.sourceGameObject.defaultValue = Parent;
           // _time = 0;
           // CurrentEffect = GameObject.Instantiate(prefabEffect,Parent.transform).GetComponent<ParticleSystem>();
            base.OnBehaviourPlay(playable, info);
        }
        
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
           // if(CurrentEffect)
           //      GameObject.DestroyImmediate(CurrentEffect.gameObject);
            base.OnBehaviourPause(playable, info);
        }
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            //CurrentEffect.Pause();
            //_time += info.deltaTime;
            //currentEffect.time = _time;
            base.PrepareFrame(playable, info);
        }

        public void OnInit(EffectExtensionPa extensionPa,GameObject prefabEffect)
        {
            this.prefabEffect = prefabEffect;
            _pa = extensionPa;
        }
    }
}