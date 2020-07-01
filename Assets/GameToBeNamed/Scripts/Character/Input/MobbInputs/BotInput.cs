using UnityEngine;
using Rewired;


namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public abstract class BotInput : InputSource {
        
        //as funções genericas 
        //ex: SetDestination, ResetDestination, SetInitialDestination, IsDestinationReached, SetNextDestination, SearchDestination

        public abstract override void Configure(Character2D character);
        
        public abstract override void Update();
        

        public abstract void SetDestination(out Transform target);
        
        public abstract bool SearchDestination();

        public abstract void MoveToDestination();

        public abstract void SetInitialDestination();

        public abstract bool IsDestinationReached();

        public abstract void SetNextDestination();
    }
}