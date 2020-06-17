using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class WaypointInfo  {
 
        public Vector3 wayPoint;
        // Use this for initialization
        public bool IsWaypointReached(Vector3 movingObject, float deadZone = 0.3f) {
            return Vector3.Distance(movingObject, wayPoint) < deadZone;
        }
    }
}