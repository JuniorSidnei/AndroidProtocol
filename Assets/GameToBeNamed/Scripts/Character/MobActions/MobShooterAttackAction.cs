using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class MobShooterAttackAction : CharacterAction {
        
        private List<PropertyName> m_unallowedStatus;

        private Character2D m_char;
        [SerializeField] private float m_attackRecharge;
        [SerializeField] private GameObject m_bullet;
        [SerializeField] private Transform m_bulletSpawn;
        private float m_attackCooldown;
        private float m_direction;
        private Vector2 m_bulletSpawmnerPosition;

        private IInputSource m_input;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;

            m_attackCooldown = m_attackRecharge;
            m_bulletSpawmnerPosition = m_bulletSpawn.transform.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.ReceivingDamage
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnExecuteAttack>(OnExecuteAttack);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Unsubscribe<OnExecuteAttack>(OnExecuteAttack);
        }


        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            m_direction = m_char.Velocity.x > 0 ? 1 : -1;
            m_attackCooldown -= Time.deltaTime;
            
            m_bulletSpawn.transform.localPosition = new Vector3(m_direction * m_bulletSpawmnerPosition.x, m_bulletSpawmnerPosition.y);
            if (m_input.HasActionDown(InputAction.Button4)  && m_attackCooldown <= 0) {
                m_char.ActionStates[ActionStates.Attacking] = true;
                m_char.Velocity = Vector2.zero;
                m_char.LocalDispatcher.Emit(new OnFirstAttack());
                m_attackCooldown = m_attackRecharge;
            }
        }

        private void OnExecuteAttack(OnExecuteAttack ev) {
           
            InstantiateController.Instance.InstantiateDirectionalEffect(m_bullet, m_bulletSpawn.position, m_direction);
            m_char.ActionStates[ActionStates.Attacking] = false;
        }
    }
}