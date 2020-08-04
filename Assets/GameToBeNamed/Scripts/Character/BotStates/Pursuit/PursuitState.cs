using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [Serializable]
    public class PursuitState : State {
        
        [SerializeField] private  int m_lineOfSite;
        private float m_targetDistance;

        public override void Enter(Character2D character, BotInput input) { }

        public override void Run(Character2D character, BotInput input) {

            m_targetDistance = Vector2.Distance(character.transform.position, input.GetDestinationPosition());

            if (m_targetDistance < m_lineOfSite) {
                input.MoveToDestination(input.GetDestinationPosition());
            }
        }

        public override void Exit(Character2D character, BotInput input) {
            input.MoveToDestination(input.GetDestinationPosition());
        }
    }
}
