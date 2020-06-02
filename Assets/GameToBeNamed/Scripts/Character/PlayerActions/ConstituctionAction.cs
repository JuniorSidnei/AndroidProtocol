using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameToBeNamed.Character {

    [Serializable]
    public class ConstituctionAction : CharacterAction {

        private Character2D m_char;
        public CharacterStatusLife characterStatusLife;
        [SerializeField] private float m_damageCooldown;
        [SerializeField] private int m_armorDefense;
        private float m_damageCooldownTimer;

        protected override void OnConfigure() {
            
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
            m_char.LocalDispatcher.Subscribe<OnReceiveDamageFinish>(OnReceiveDamageFinish);
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterConfigureConstitution(characterStatusLife.MaxHealth, characterStatusLife.CurrentHealth, characterStatusLife.IconSplash, characterStatusLife.LifeSplash));
        }

        private void OnReceiveDamageFinish(OnReceiveDamageFinish ev) {
            m_char.ActionStatus.Remove(ActionStates.ReceivingDamage);
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {
            
            if (m_damageCooldownTimer > Time.time) {
                return;
            }

            m_char.ActionStatus[ActionStates.ReceivingDamage] = true;
            
            m_damageCooldownTimer = Time.time + m_damageCooldown;
            characterStatusLife.CurrentHealth -= ev.Damage - m_armorDefense;
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDamage(ev.Damage - m_armorDefense, m_char.transform.position, characterStatusLife.CurrentHealth, characterStatusLife.MaxHealth));

            if (characterStatusLife.CurrentHealth <= 0) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDeath(m_char, characterStatusLife.OnAfterDeserialize));
            }
        }
    }
}