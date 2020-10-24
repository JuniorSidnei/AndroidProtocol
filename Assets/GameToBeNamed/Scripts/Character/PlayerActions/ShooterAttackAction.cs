using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Character{

    [System.Serializable]
    public class ShooterAttackAction : CharacterAction {
        
        
        [SerializeField] private float m_fireRate;
        [SerializeField] private GameObject m_bullet;
        [SerializeField] private Transform m_bulletSpawn;
        [SerializeField] private int m_startAmmunitionAmount;
        [SerializeField] private int m_ammunitionAmount;
        [SerializeField] private SpriteRenderer m_spriteVfx;
        private List<PropertyName> m_unallowedStatus;
        
        private IInputSource m_input;
        private Character2D m_char;
        private Vector2 m_shootPosition;
        private int m_direction;
        
        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            m_ammunitionAmount = m_startAmmunitionAmount;
            m_shootPosition = m_bulletSpawn.localPosition;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage    
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        
        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (Character2D.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            SetDirection();
            
            if (m_ammunitionAmount <= 0) {
                m_fireRate = 4f;
                m_ammunitionAmount = m_startAmmunitionAmount;
            }
            
            if (m_input.HasActionDown(InputAction.Button4) &&  m_fireRate < 0) {

                m_char.Velocity = Vector2.zero;
                m_bulletSpawn.localPosition = new Vector3(m_direction * m_shootPosition.x, m_shootPosition.y);
                InstantiateController.Instance.InstantiateDirectionalEffect(m_bullet, m_bulletSpawn.position, m_direction);
                m_fireRate = 1f;
                m_ammunitionAmount -= 1;
                m_char.LocalDispatcher.Emit(new OnFirstAttack());
            }
            
            m_fireRate -= Time.deltaTime;
        }

        private void SetDirection() {
            
            if (m_spriteVfx.flipX) {
                m_direction = -1;
            }
            else {
                m_direction = 1;
            }
        }
    }
}