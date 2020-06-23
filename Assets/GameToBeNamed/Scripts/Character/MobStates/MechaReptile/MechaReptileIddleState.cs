using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    //aqui só vai perguntar com está a situação dos waypoints e tudo mais, só isso
    [Serializable]
    public class MechaReptileIddleState : State {

        public int idState;
        public override void Enter(Character2D character, BotInput input) {
            
           
//            if (wayPoints.Count <= 0) {
//                throw new Exception("Nenhum waypoint setado");
//            }
            
           
           
        }

        public override void Run(Character2D character, BotInput input) {

//            if (currentWayPoint == wayPoints[0]) {
//                input.SetAction(InputAction.Button2);
//            }
//            
//            if (currentWayPoint.IsWaypointReached(character.transform.position)) {
//                NextWaypoint(input);
//            }
        }

        public override void Exit(Character2D character, BotInput input) {
            Debug.Log("Sai do iddle");
        }
        
        
    }
}