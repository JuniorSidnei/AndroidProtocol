using GameToBeNamed.Character.Data;
using UnityEngine;

namespace GameToBeNamed.Character {

    public interface ICharacterAction {
        void Configure(Character2D character2D);
        void Activate();
        void Deactivate();
        
    }
    
    public abstract class CharacterAction : ICharacterAction {

        protected Character2D Character2D;
        public bool IsActionActive;
       
        public void Configure(Character2D character2D) {
            Character2D = character2D;
            OnConfigure();
        }

        public void Activate( ) {
            IsActionActive = true;
            OnActivate();
        }

        public void Deactivate() {
            IsActionActive = false;
            OnDeactivate();
        }

        protected abstract void OnConfigure();
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}