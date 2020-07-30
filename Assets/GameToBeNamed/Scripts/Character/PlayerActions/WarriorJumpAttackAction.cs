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
        
        [SerializeField] private int m_damage;
        [SerializeField] private Collision2DProxy m_attackBox;
        private Vector2 m_attackBoxPosition;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        private float m_direction;
        private Character2D m_char;
        private float m_comboIntervalTimer;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnWarriorAirAttack>(OnWarriorAirAttack);
            m_char.LocalDispatcher.Subscribe<OnAttackFinish>(OnAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
            m_attackBoxPosition = m_attackBox.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage   
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (Character2D.ActionStates.AllNotDefault(m_unallowedStatus).Any() || m_char.Controller2D.collisions.below || m_char.Controller2D.collisions.left
                || m_char.Controller2D.collisions.right) {
                return;
            }


            if (m_char.Input.HasActionDown(InputAction.Button4) || !m_char.Controller2D.collisions.below && m_char.Velocity.y > 2) {
               
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.LocalDispatcher.Emit(new OnJumpAttack());
                SetDirection();
            }
        }

        private void OnTrigger2DEnterCallback(Collider2D collider) {
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char, Receiver = collider.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, collider.bounds.center));
        }
        
        private void OnWarriorAirAttack(OnWarriorAirAttack ev) {
            m_attackBox.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y,0);
        }
        
        private void OnAttackFinish(OnAttackFinish ev) {
            m_attackBox.enabled = false;
            m_char.ActionStates[ActionStates.Attacking] = false;
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
