using UnityEngine;

namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class MoveAction : CharacterAction {
        
        public Character2D.Status UnallowedStatus;
        public float Speed = 200;
        public float InAirDrag = 0.5f, InGroundDrag = 5;

        private IInputSource m_input;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }
        
        private void OnCharacterUpdate(OnCharacterUpdate ev)
        {

            if (Character2D.HasStatus(UnallowedStatus)) {
                return;
            }

            if (Character2D.Controller2D.collisions.below) {
                Character2D.Drag = InGroundDrag;
            }
            else if (!Character2D.Controller2D.collisions.below) {
                Character2D.Drag = InAirDrag;
            }
            
            
            if (m_input.HasAction(InputAction.Button2)) {//right
                Character2D.Velocity += new Vector2(Speed * Time.deltaTime, 0);
                Character2D.SetStatus(Character2D.Status.Moving);
            }
            
            if (m_input.HasAction(InputAction.Button3)) {//left
                Character2D.Velocity -= new Vector2(Speed * Time.deltaTime, 0);
                Character2D.SetStatus(Character2D.Status.Moving);
            }
        }
    }
}