using UnityEngine;

namespace CharacterSystem {
    
    [System.Serializable]
    public class MoveAction : CharacterAction {
        
        public Character2D.Status UnallowedStatus;
        public float Speed = 50;
        public float InAirDrag = 0.5f, InGroundDrag = 14;
        
        private IInputSource m_input;
        private Rigidbody2D m_rb;
        
        protected override void OnConfigure() {
            m_input = Character2D.Input;
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_rb = Character2D.GetComponent<Rigidbody2D>();
        }
        
        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            if (Character2D.HasStatus(UnallowedStatus)) {
                return;
            }
            
            m_rb.drag = Character2D.HasStatus(Character2D.Status.OnGround) ? InGroundDrag : InAirDrag;
            
            if (m_input.HasAction(InputAction.Button2)) {//right
                m_rb.AddForce(Speed * Time.deltaTime * Vector3.right);
            }
            if (m_input.HasAction(InputAction.Button3)) {//left
                m_rb.AddForce(Speed * Time.deltaTime * Vector3.left);
            }
        }
    }
}