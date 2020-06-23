using UnityEngine;
using Rewired;


namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public abstract class BotInput : InputSource {
        
        public override void Configure(Character2D character) { }

        public override void Update() { }
    }
}