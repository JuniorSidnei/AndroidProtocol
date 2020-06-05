using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {

    public class MechaReptileIddleState : CharacterState<Character2D> {

        struct WorldData {
            private Vector3 m_target;
        }

        public override void OnConfigure(Character2D character) {
        }

        public override void Enter(Character2D character) {}

        public override void Run(Character2D character) {}

        public override void Exit(Character2D character) {}
        
        public override void Update(Character2D character) {
            
        }
    }
}