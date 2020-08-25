using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class MobAttackAction : CharacterAction {
        
        private List<PropertyName> m_unallowedStatus;

        private Character2D m_char;
        [SerializeField] private int m_damage;
        [SerializeField] private float m_attackCooldown;
        [SerializeField] private Collision2DProxy m_attackBox;
        private Vector2 m_attackBoxPosition;
        private float m_direction;

        private IInputSource m_input;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;

            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnExecuteAttack>(OnAttack);
            m_char.LocalDispatcher.Subscribe<OnSecondAttackFinish>(OnAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
            m_attackBoxPosition = m_attackBox.transform.localPosition;

            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.ReceivingDamage
            };
        }

        

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            m_direction = m_char.Velocity.x > 0 ? 1 : -1;
            m_attackCooldown -= Time.deltaTime;
            
            if (m_input.HasActionDown(InputAction.Button4)  && m_attackCooldown <= 0) {
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.LocalDispatcher.Emit(new OnFirstAttack());
                m_attackCooldown = 2f;
            }
        }

        private void OnAttack(OnExecuteAttack ev) {
            m_attackBox.BoxCollider.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y, 0);
        }

        private void OnAttackFinish(OnSecondAttackFinish ev) {
            m_attackBox.BoxCollider.enabled = false;
            m_char.ActionStates[ActionStates.Attacking] = false;
        }

        private void OnTrigger2DEnterCallback(Collider2D ev) {
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char.gameObject, Receiver = ev.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, ev.bounds.center));
        }
    }
}