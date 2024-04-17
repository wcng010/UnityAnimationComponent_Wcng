using UnityEngine;
using UnityEngine.Timeline;
using Wcng.FSM;

namespace Wcng.Library
{
    public class SignalExtensionSubscriber : MonoBehaviour
    {
        public StateLoaderSo stateLoaderSo;
        [SerializeField]private SignalReceiver signalReceiver;
        private void Awake()
        {
            foreach (var state in stateLoaderSo.states)
            {
                if (state.MyEventSignals != null)
                {
                    foreach (var stateEvent in state.MyEventSignals)
                    {
                        foreach (var eventTemp in stateEvent.Key.events)
                        {
                            signalReceiver.AddReaction(stateEvent.Key.signalAsset, eventTemp);
                        }
                    }
                }
            }
        }
    }
}