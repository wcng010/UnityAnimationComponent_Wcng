using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wcng.FSM
{
    [CreateAssetMenu(fileName = "StateLoader",menuName = "Data/State/StateLoader")]
    public class StateLoaderSo : ScriptableObject
    {
        public List<CharacterState> states = new List<CharacterState>();
        public int MaxIndex => states.Count;
        
        public CharacterState GetState(Type stateType)
        {
            foreach (var state in states)
            {
                if (state.GetType() == stateType)
                {
                    return state;
                }
            }

            return null;
        }
    }
}
