using GameToBeNamed.Character.Data;
using UnityEngine;

namespace GameToBeNamed.Character {

    public interface ICharacterAction {
        void Configure(Character2D character2D);
        void Activate();
        void Deactivate();

        string GetName();

    }
    
    public abstract class CharacterAction : ICharacterAction {

        protected Character2D Character2D;
        public bool IsActionActive;
        protected string m_name;
       
        public void Configure(Character2D character2D) {
            Character2D = character2D;
            m_name = GetType().Name;
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

        public string GetName() {
            return m_name;
        }
        
        protected abstract void OnConfigure();
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}