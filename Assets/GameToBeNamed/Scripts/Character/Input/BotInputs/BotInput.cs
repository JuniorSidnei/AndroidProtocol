using System;
using UnityEngine;
using Rewired;


namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public abstract class BotInput : InputSource {
        
        //as funções genericas 
        //ex: SetDestination, ResetDestination, SetInitialDestination, IsDestinationReached, SetNextDestination, SearchDestination

        public abstract override void Configure(Character2D character);
        
        public abstract override void Update();
        
        public abstract void SetTarget(GameObject target);
        
        public abstract Vector3 GetDestinationPosition();

        public abstract bool SearchDestination();

        public abstract void MoveToDestination(Vector3 destination);

        public abstract void SetInitialDestination();

        public abstract bool IsDestinationReached(Vector3 target);

        public abstract bool IsTargetClose(Vector3 target);

        public abstract bool IsTargetSet();

        public abstract void SetNextMovement();

        public abstract void SetRunMovement();

        public abstract void SetAttackAction();

    }
}