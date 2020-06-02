using UnityEngine;
using Rewired;


namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class BotInput : InputSource {
        
        public int BotID = 0;
        
        public override void Configure() { }
        public override void Update() { }
    }
}