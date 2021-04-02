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
        public float delayToAttack = 0.5f;
        
        public override void Enter(Character2D character, BotInput input) {
            Debug.LogError("Entrei no run");
            
            if (character.transform.position.x < input.GetDestinationPosition().x) {
                m_newDestination = character.transform.position - new Vector3(distanceToRun, 0, 0);
            }
            else {
                m_newDestination = character.transform.position + new Vector3(distanceToRun, 0, 0);
            }
        }

        public override void Run(Character2D character, BotInput input) {
            
            input.MoveToDestination(m_newDestination);
            
            if (input.IsDestinationReached(m_newDestination)) {
                Debug.LogError("cheguei no destino de correr");
            }
        }

        public override void Exit(Character2D character, BotInput input) {
            Debug.LogError("Sai do run");
        }
    }
}