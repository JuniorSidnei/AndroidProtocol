using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
    [System.Serializable]
    public class RogueJumpAttackAction : CharacterAction {

        private List<PropertyName> m_unallowedStatus;
        
        private IInputSource m_input;
        private Character2D m_char;
        [SerializeField] private float m_damage;
        [SerializeField] private Collision2DProxy[] m_attackBoxes;
        [SerializeField] private Vector2[] m_attackBoxesPositions;
        private float m_direction;

        protected override void OnConfigure() {
            m_char = Character2D;
            m_input = m_char.Input;
           
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnRogueAirAttack>(OnRogueAirAttack);
            m_char.LocalDispatcher.Subscribe<OnSecondAttackFinish>(OnAttackFinish);

            for (var i = 0; i < m_attackBoxes.Length; i++) {
                m_attackBoxesPositions[i] = m_attackBoxes[i].transform.localPosition;
            }
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage    
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any() || m_char.Controller2D.collisions.below || m_char.Controller2D.collisions.left
                || m_char.Controller2D.collisions.right) {
                return;
            }
            
            m_direction = m_char.Velocity.x > 0 ? 1 : -1;
            
            if (m_input.HasActionDown(InputAction.Button4) && !m_char.Controller2D.collisions.below &&
                m_char.Velocity.y > 2) {
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.LocalDispatcher.Emit(new OnJumpAttack());
            }
        }
        
        private void OnAttackFinish(OnSecondAttackFinish ev) {
            m_char.ActionStates[ActionStates.Attacking] = false;
            foreach (var boxes in m_attackBoxes) {
                boxes.BoxCollider.enabled = false;
            }
        }

        private void OnRogueAirAttack(OnRogueAirAttack ev) {

            for (var i = 0; i < m_attackBoxes.Length; i++) {
                m_attackBoxes[i].BoxCollider.enabled = true;
                m_attackBoxes[i].transform.localPosition = new Vector3(m_direction * m_attackBoxesPositions[i].x, m_attackBoxesPositions[i].y,0);
            }
        }
    }
}