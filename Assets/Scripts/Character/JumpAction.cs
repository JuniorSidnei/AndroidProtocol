using UnityEngine;

namespace CharacterSystem {

    [System.Serializable]
    public class JumpAction : CharacterAction {
        
        public Character2D.Status UnallowedStatus;
        public float JumpForce;
        private Rigidbody2D m_rb;
        
        protected override void OnConfigure() {
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_rb = Character2D.GetComponent<Rigidbody2D>();
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            if (Character2D.HasStatus(UnallowedStatus) || !Character2D.HasStatus(Character2D.Status.OnGround)) {
                return;
            }
            
            if (Character2D.Input.HasActionDown(InputAction.Button1)) {
                m_rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            }
        }
    }
}