using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Character{

    [System.Serializable]
    public class RogueAttackAction : CharacterAction {
        
        [SerializeField] private int m_damage;
        [SerializeField] private Collision2DProxy  m_attackBox;
        private Vector2 m_attackBoxPosition;
        private float m_direction;
        private int m_comboStep;
        private float m_comboIntervalTimer;
        [SerializeField] private float m_damageCooldown; 
        private float m_damageCooldownTimer;

        private List<PropertyName> m_unallowedStatus;
        
        private IInputSource m_input;
        private Character2D m_char;
        
        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            
            m_attackBox.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
            m_char.LocalDispatcher.Subscribe<OnAttack>(OnAttack);
            m_char.LocalDispatcher.Subscribe<OnAttackFinish>(OnAttackFinish);
            
            m_attackBoxPosition = m_attackBox.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage    
            };
        }

        private void OnTrigger2DEnterCallback(Collider2D collider) {
            
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char, Receiver = collider.gameObject
            };
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, collider.bounds.center));
        }

        
        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (Character2D.ActionStatus.AllNotDefault(m_unallowedStatus).Any() || !m_char.Controller2D.collisions.below) {
                return;
            }

            m_direction = m_char.Velocity.x > 0 ? 1 : -1;
            
            if (m_input.HasActionDown(InputAction.Button4) && (m_comboStep == 0  || m_comboIntervalTimer < 0)) {
                
                m_char.LocalDispatcher.Emit(new OnFirstAttack());
                m_comboStep = 1;
                m_comboIntervalTimer = 2f;
            }
            else if (m_input.HasActionDown(InputAction.Button4)
                     && (m_comboStep == 1 && m_comboIntervalTimer >= 1f)) {
                
                m_char.LocalDispatcher.Emit(new OnSecondAttack());
                m_comboStep = 2;
            }
            else if (m_input.HasActionDown(InputAction.Button4)
                     && (m_comboStep == 2 && m_comboIntervalTimer >= 0)) {
                
                m_char.LocalDispatcher.Emit(new OnThirdAttack());
                m_comboStep = 0;
            }

            m_comboIntervalTimer -= Time.deltaTime;
        }

        private void OnAttack(OnAttack ev) {
            m_attackBox.Collider.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y,0);
        }

        private void OnAttackFinish(OnAttackFinish ev) {
            m_attackBox.Collider.enabled = false;
        }

    }
}