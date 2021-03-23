using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    
    [Serializable]
    public class RunState : State {

        public int id;

        private Vector3 m_newDestination;
        public float distanceToRun = 0;
        
        public override void Enter(Character2D character, BotInput input) {
            
            if (character.transform.position.x < input.GetDestinationPosition().x) {
                m_newDestination = input.GetDestinationPosition() + new Vector3(distanceToRun, 0, 0);
            }
            else {
                m_newDestination = input.GetDestinationPosition() + new Vector3(-distanceToRun, 0, 0);
            }
        }

        public override void Run(Character2D character, BotInput input) {
            
            if (input.IsTargetClose(input.GetDestinationPosition())) {
                input.MoveToDestination(m_newDestination);
            }
            else {
                input.SetAttackAction();
            }
        }

        public override void Exit(Character2D character, BotInput input) {
        }
    }
}