using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameToBeNamed.Character {

    [Serializable]
    public class ConstituctionAction : CharacterAction {

        private Character2D m_char;
        private List<PropertyName> m_unallowedStatus;
        public CharacterStatusLife characterStatusLife;
        [SerializeField] private Vector2 m_knockbackForce;
        [SerializeField] private float m_damageCooldown;
        [SerializeField] private int m_armorDefense;
        [SerializeField] private GameObject m_hitEffect;
        private float m_damageCooldownTimer;

        protected override void OnConfigure() {
            
            m_char = Character2D;

            m_unallowedStatus  = new List<PropertyName>() {
                ActionStates.Blocking
            };
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterConfigureConstitution(characterStatusLife.MaxHealth, characterStatusLife.CurrentHealth, characterStatusLife.IconSplash, characterStatusLife.LifeSplash));
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnReceivedAttack>(OnReceivedAttack);
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {
            
            if (m_damageCooldownTimer > Time.time ||  m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            m_char.ActionStates[ActionStates.ReceivingDamage] = true;
            characterStatusLife.CurrentHealth -= ev.Damage - m_armorDefense;
            m_damageCooldownTimer = Time.time + m_damageCooldown;
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDamage(ev.Damage - m_armorDefense, m_char.transform.position, characterStatusLife.CurrentHealth, characterStatusLife.MaxHealth));

            if (ev.AttackInfo.Emiter.transform.position.x > m_char.transform.position.x) {
                m_char.Velocity = new Vector2(-m_knockbackForce.x, m_knockbackForce.y);
            }
            else {
                m_char.Velocity = new Vector2(m_knockbackForce.x, m_knockbackForce.y);
            }
            
            InstantiateController.Instance.InstantiateEffect(m_hitEffect, ev.DamageContact);
            var to = m_char.Velocity;
            DOTween.To(() => m_char.Velocity, x => m_char.Velocity = to, to, .2f).SetEase(Ease.Linear).OnComplete(()=> {
                m_char.ActionStates[ActionStates.ReceivingDamage] = false;
            });
            
            if (characterStatusLife.CurrentHealth <= 0) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDeath(m_char, characterStatusLife.OnAfterDeserialize));
            }
        }
    }
}