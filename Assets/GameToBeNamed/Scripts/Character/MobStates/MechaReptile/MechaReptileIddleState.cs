using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToBeNamed.Character {
    
    [Serializable]
    public class MechaReptileIddleState : State {

        class MechaReptileData {
            
            public float RightRangeData;
            public float LeftRangeData;
        }
      
        public WaypointInfo[] wayPoints;
        private WaypointInfo currentWayPoint;
        private int currentWayPointIndex;

        public override void Enter(Character2D character, BotInput input) {

            var reptileData = DataHolder.Instance.GetDataOrCreateForCharacter<MechaReptileData>(character);


            if (wayPoints.Length <= 0) {
                throw new Exception("Nenhum waypoint setado");
            }
            
            currentWayPoint = wayPoints[0];
            currentWayPointIndex = 0;
            input.SetAction(InputAction.Button2);
        }

        public override void Run(Character2D character, BotInput input) {

            //os inputs funcionan no bot, só precisa calibrar as condições
            var reptileData = DataHolder.Instance.GetDataOrCreateForCharacter<MechaReptileData>(character);

            if (currentWayPoint.IsWaypointReached(character.transform.position)) {
                NextWaypoint(input);
            }
        }

        public override void Exit(Character2D character, BotInput input) {
            Debug.Log("Sai do iddle");
        }
        
        private void NextWaypoint(BotInput input) {
            currentWayPointIndex++;
            
            if (currentWayPointIndex >= wayPoints.Length - 1) {
                currentWayPointIndex = 0;
            }
 
            currentWayPoint = wayPoints[currentWayPointIndex];
        }
    }
}