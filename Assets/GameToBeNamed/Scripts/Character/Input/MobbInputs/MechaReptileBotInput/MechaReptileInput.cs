using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class MechaReptileInput : BotInput {
        
        [SerializeField] private StateMachine m_stateMachine;

        private Character2D m_char;
        
        [SerializeField] private List<Vector3> m_wayPoints;
        private Vector3 currentWayPoint;
        private int currentWayPointIndex;
        
        public override void Configure(Character2D character) {
            m_char = character;
            m_stateMachine.OnConfigure();
            m_stateMachine.ChangeState(m_char, new MechaReptileIddleState(), this);
            
            currentWayPoint = m_wayPoints[0];
            currentWayPointIndex = 0;
        }

        public override void Update() {
            m_stateMachine.OnUpdate(m_char, this);
        }
        
        public bool IsWaypointReached(Vector3 movingObject, float deadZone = 0.3f) {
            return Vector3.Distance(movingObject, currentWayPoint) < deadZone;
        }
        
        private void NextWaypoint() {
            currentWayPointIndex++;
            
            if (currentWayPointIndex >= m_wayPoints.Count - 1) {
                currentWayPointIndex = 0;
            }
 
            currentWayPoint = m_wayPoints[currentWayPointIndex];
        }
    }
}