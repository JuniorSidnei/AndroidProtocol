using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    
    [Serializable]
    public class AttackState : State {

        public int id;
        private bool m_activated = false;
        
        public override void Enter(Character2D character, BotInput input) {
            input.SetAttackAction();
            m_activated = true;
            id = 10;
        }

        public override void Run(Character2D character, BotInput input) {
            
            input.SetAttackAction();
            
            if (input.IsTargetClose(input.GetDestinationPosition())) {
                input.SetRunMovement();
            }
        }

        public override void Exit(Character2D character, BotInput input) {
            m_activated = false;
            id = 0;
        }
    }
}