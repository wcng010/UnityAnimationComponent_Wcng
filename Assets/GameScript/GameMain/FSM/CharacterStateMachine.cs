using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.Playables;

namespace Wcng
{
    public class CharacterStateMachine : StateMachine<CharacterState>
    {
        public CharacterStateMachine()
        {
            
        }

        public CharacterStateMachine(InputComponent inputComponent,AnimancerComponent animancer,PlayableDirector playableDirector)
        {
            InputComponent = inputComponent;
            AnimancerComponent = animancer;
            PlayableDirector = playableDirector;
        }
    }
}
