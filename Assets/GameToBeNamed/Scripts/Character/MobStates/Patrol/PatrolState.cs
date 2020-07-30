using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    
    [Serializable]
    public class PatrolState : State {

        public int id; 

        public override void Enter(Character2D character, BotInput input) {
            input.SetInitialDestination();
        }

        public override void Run(Character2D character, BotInput input) {
            
            input.MoveToDestination();
            
            if (input.IsDestinationReached()) {
                input.SetNextDestination();
            }
        }

        public override void Exit(Character2D character, BotInput input) {
            Debug.Log("Sai do iddle");
        }
        
    }
}