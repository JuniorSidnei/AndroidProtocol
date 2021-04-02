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
            Debug.LogError("Entrei do attack");
            input.SetAttackAction();
            id = 10;
        }

        public override void Run(Character2D character, BotInput input) {
            
            input.SetAttackAction();
            
            if (input.IsTargetClose(input.GetDestinationPosition())) {
                Debug.LogError("Vou correr");
                input.SetRunMovement();
            }
        }

        public override void Exit(Character2D character, BotInput input) {
            Debug.LogError("Sai do attack");
            id = 0;
        }
    }
}