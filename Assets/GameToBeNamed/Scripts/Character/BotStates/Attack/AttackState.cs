using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    
    [Serializable]
    public class AttackState : State {

        public int id; 

        public override void Enter(Character2D character, BotInput input) {
            input.SetAttackAction();
        }

        public override void Run(Character2D character, BotInput input) {
            
            if (input.IsTargetClose(input.GetDestinationPosition())) {
                input.SetRunMovement();
            }
        }

        public override void Exit(Character2D character, BotInput input) {
        }
    }
}