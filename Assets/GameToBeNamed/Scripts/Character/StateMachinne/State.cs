using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    public interface IState {
         void Enter(Character2D character, BotInput input);
         void Run(Character2D character, BotInput input);
         void Exit(Character2D character, BotInput input);
    }
    
    public abstract class State : IState {
        
        public abstract void Enter(Character2D character, BotInput input);
        public abstract void Run(Character2D character, BotInput input);
        public abstract void Exit(Character2D character, BotInput input);
    }
}