using UnityEngine;

namespace GameToBeNamed.Character {

    public interface ICharacterAction {
        void Configure(Character2D character2D);
    }
    
    public abstract class CharacterAction : ICharacterAction {

        protected Character2D Character2D;
        
        public void Configure(Character2D character2D) {
            Character2D = character2D;
            OnConfigure();
        }

        protected abstract void OnConfigure();
    }
}