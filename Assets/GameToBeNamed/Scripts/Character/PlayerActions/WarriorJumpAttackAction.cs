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
        [SerializeField] private SpriteRenderer m_spriteVfx;
        private float m_direction;
        private Character2D m_char;
        private IInputSource m_input;
        private float m_comboIntervalTimer;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_input = m_char.Input;
            m_attackBoxPosition = m_attackBox.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage   
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnWarriorAirAttack>(OnWarriorAirAttack);
            m_char.LocalDispatcher.Subscribe<OnSecondAttackFinish>(OnAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Unsubscribe<OnWarriorAirAttack>(OnWarriorAirAttack);
            m_char.LocalDispatcher.Unsubscribe<OnSecondAttackFinish>(OnAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.RemoveListener(OnTrigger2DEnterCallback);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any() || m_char.Controller2D.collisions.below || m_char.Controller2D.collisions.left
                || m_char.Controller2D.collisions.right) {
                return;
            }


            if (m_input.HasActionDown(InputAction.Button4) && !m_char.Controller2D.collisions.below) {
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.LocalDispatcher.Emit(new OnJumpAttack());
                SetDirection();
            }
        }

        private void OnTrigger2DEnterCallback(Collider2D collider) {
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char.gameObject, Receiver = collider.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, collider.bounds.center));
        }
        
        private void OnWarriorAirAttack(OnWarriorAirAttack ev) {
            m_attackBox.BoxCollider.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y,0);
        }
        
        private void OnAttackFinish(OnSecondAttackFinish ev) {
            m_attackBox.BoxCollider.enabled = false;
            m_char.ActionStates[ActionStates.Attacking] = false;
        }

        private void SetDirection() {
            
            if (m_spriteVfx.flipX) {
                m_direction = -1;
            }
            else if(!m_spriteVfx.flipX) {
                m_direction = 1;
            }
        }
    }
}
