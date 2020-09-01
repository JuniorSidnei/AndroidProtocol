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
        private Vector2 m_attackBoxPosition;
        private float m_direction;

        private IInputSource m_input;
        
        private int m_comboStep;
        private float m_comboIntervalTimer;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            m_attackBoxPosition = m_attackBox.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage, ActionStates.Jumping  
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Unsubscribe<OnExecuteAttack>(OnExecuteAttack);
            m_char.LocalDispatcher.Unsubscribe<OnFirstAttackFinish>(OnFirstAttackFinish);
            m_char.LocalDispatcher.Unsubscribe<OnSecondAttackFinish>(OnSecondAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.RemoveListener(OnTrigger2DEnterCallback);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Unsubscribe<OnExecuteAttack>(OnExecuteAttack);
            m_char.LocalDispatcher.Unsubscribe<OnFirstAttackFinish>(OnFirstAttackFinish);
            m_char.LocalDispatcher.Unsubscribe<OnSecondAttackFinish>(OnSecondAttackFinish);
            m_attackBox.OnTrigger2DEnterCallback.RemoveListener(OnTrigger2DEnterCallback);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            m_direction = m_char.Velocity.x > 0 ? 1 : -1;
            m_comboIntervalTimer -= Time.deltaTime;

            if (m_comboIntervalTimer <= 0) {
                m_comboStep = 0;
            }
            
            if (m_input.HasActionDown(InputAction.Button4) && (m_comboStep == 0  && m_comboIntervalTimer < 0)) {
                m_comboStep = 1;
                m_comboIntervalTimer = 1;
                m_char.LocalDispatcher.Emit(new OnFirstAttack());
            }
            else if (m_input.HasActionDown(InputAction.Button4) && (m_comboStep == 1 && m_comboIntervalTimer >= 0)) {
                m_char.LocalDispatcher.Emit(new OnSecondAttack());
                m_comboStep = 0;
            }
        }
        
        private void OnTrigger2DEnterCallback(Collider2D collider) {
            
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char.gameObject, Receiver = collider.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damage, collider.bounds.center));
        }

        private void OnExecuteAttack(OnExecuteAttack ev) {
            m_char.ActionStates[ActionStates.Attacking] = true;
            m_attackBox.BoxCollider.enabled = true;
            m_attackBox.transform.localPosition = new Vector3(m_direction * m_attackBoxPosition.x, m_attackBoxPosition.y,0);
        }
        
        private void OnFirstAttackFinish(OnFirstAttackFinish ev) {
            m_attackBox.BoxCollider.enabled = false;
            m_char.ActionStates[ActionStates.Attacking] = false;
            GameManager.Instance.GlobalDispatcher.Emit(new OnCameraScreenshake(1, .2f));
        }
        
        private void OnSecondAttackFinish(OnSecondAttackFinish ev) {
            m_attackBox.BoxCollider.enabled = false;
            m_char.ActionStates[ActionStates.Attacking] = false;
        }
    }
}
