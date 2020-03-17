using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class JumpAction : CharacterAction {
        
        public Character2D.Status UnallowedStatus;
        public float JumpForce;
        public float Gravity;

        protected override void OnConfigure() {
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            Character2D.Velocity += new Vector2(0, Gravity * Time.deltaTime);
            
            if (Character2D.HasStatus(UnallowedStatus) || !Character2D.Controller2D.collisions.below) {
                return;
            }
            
            if (Character2D.Input.HasActionDown(InputAction.Button1)) {
                Character2D.Velocity += Vector2.up * JumpForce;
                Character2D.SetStatus(Character2D.Status.Jumping);
            }
        }
    }
}