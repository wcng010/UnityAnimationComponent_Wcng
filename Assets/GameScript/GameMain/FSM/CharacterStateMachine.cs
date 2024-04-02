using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace Wcng
{
    public class CharacterStateMachine : StateMachine<CharacterState>
    {
        public CharacterStateMachine()
        {
            
        }

        public CharacterStateMachine(InputComponent inputComponent,AnimancerComponent animancer)
        {
            InputComponent = inputComponent;
            AnimancerComponent = animancer;
        }
    }
}
