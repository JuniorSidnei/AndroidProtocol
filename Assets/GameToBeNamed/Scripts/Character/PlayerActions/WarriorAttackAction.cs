using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{
    [System.Serializable]
    public class WarriorAttackAction : CharacterAction {

        private List<PropertyName> m_unallowedStatus;
        
        private Character2D m_char;
        [SerializeField] private int m_damage;
        [SerializeField] private Collision2DProxy m_attackBox;
        [SerializeField] private float m_velocityAttackForce;
        private Vector2 m_attackBoxPosition;
        private float m_direction;

        private IInputSource m_input;
        
        private int m_comboStep;
        private float m_comboIntervalTimer;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnAttack>(OnAttack);
            m_char.LocalDispatcher.Subscribe<OnStrike>(OnStrike);
            m_char.LocalDispatcher.Subscribe<OnAttackFinish>(OnAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
            m_attackBoxPosition = m_attackBox.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage  
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any() || !m_char.Controller2D.collisions.below) {
                return;
            }

            m_direction = m_char.Velocity.x > 0 ? 1 : -1;
            
            if (m_input.HasActionDown(InputAction.Button4) && (m_comboStep == 0  && m_comboIntervalTimer < 0)) {
                
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.LocalDispatcher.Emit(new OnFirstAttack());
                m_comboStep = 1;
                m_comboIntervalTimer = 1;
            }
            else if (m_input.HasActionDown(InputAction.Button4) && (m_comboStep == 1 && m_comboIntervalTimer >= 0)) {
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.LocalDispatcher.Emit(new OnSecondAttack());
                m_comboStep = 0;
            }

            m_comboIntervalTimer -= Time.deltaTime;
        }
        
        private void OnTrigger2DEnterCallback(Collider2D collider) {

           
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char, Receiver = collider.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, collider.bounds.center));
        }

        private void OnAttack(OnAttack ev) {
            
            m_attackBox.BoxCollider.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y,0);
        }
        
        private void OnStrike(OnStrike ev) {
            m_char.Velocity.x += m_velocityAttackForce * m_direction;
        }

        private void OnAttackFinish(OnAttackFinish ev) {
            m_attackBox.BoxCollider.enabled = false;
            m_char.ActionStates[ActionStates.Attacking] = false;
        }
        
    }
}
