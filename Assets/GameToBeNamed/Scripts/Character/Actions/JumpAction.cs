using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class JumpAction : CharacterAction {
        
        public List<PropertyName> UnallowedStatus;
        public float JumpForce;
        private Rigidbody2D m_rb;
        
        public static PropertyName Jumping = new PropertyName("Jumping");
        
        protected override void OnConfigure() {
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_rb = Character2D.GetComponent<Rigidbody2D>();
            Character2D.ActionStatus.Add(Jumping, false);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            if (Character2D.ActionStatus.AllNotDefault(UnallowedStatus).Any()) {
                return;
            }
            
            if (Character2D.Input.HasActionDown(InputAction.Button1)) {
                Character2D.ActionStatus[Jumping] = true;
                m_rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            }
        }
    }
}