using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{
    [System.Serializable]
    public class WarriorJumpAttackAction : CharacterAction {
        private List<PropertyName> m_unallowedStatus;
        
        [SerializeField] private float Damage;
        [SerializeField] private BoxCollider2D m_attackBox;
        private Vector2 m_attackBoxPosition;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        private float m_direction;
        private Character2D m_char;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnWarriorAirAttack>(OnWarriorAirAttack);
            m_char.LocalDispatcher.Subscribe<OnAttackFinish>(OnAttackFinish);
            
            m_attackBoxPosition = m_attackBox.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage    
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (Character2D.ActionStatus.AllNotDefault(m_unallowedStatus).Any() || m_char.Controller2D.collisions.below || m_char.Controller2D.collisions.left
                || m_char.Controller2D.collisions.right) {
                return;
            }


            if (m_char.Input.HasActionDown(InputAction.Button4) || m_char.Controller2D.collisions.below) {
                
                m_char.LocalDispatcher.Emit(new OnJumpAttack());
                SetDirection();
            }
        }

        private void OnWarriorAirAttack(OnWarriorAirAttack ev) {
            m_attackBox.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y,0);
        }
        
        private void OnAttackFinish(OnAttackFinish ev) {
            m_attackBox.enabled = false;
        }

        private void SetDirection() {
            
            if (m_spriteRenderer.flipX) {
                m_direction = -1;
            }
            else if(!m_spriteRenderer.flipX) {
                m_direction = 1;
            }
        }
    }
}
