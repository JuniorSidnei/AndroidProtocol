using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameToBeNamed.Character {

    [Serializable]
    public class RechargeBatteryAction : CharacterAction {

        private IInputSource m_input;
        private Character2D m_char;
        private List<PropertyName> m_unallowedStatus;
        public CharacterStatusLife characterStatusLife;
        public GameObject RecoveryEffect;


        protected override void OnConfigure() {
            
            m_input = Character2D.Input;
            m_char = Character2D;

            m_unallowedStatus  = new List<PropertyName>() {
                ActionStates.Blocking, ActionStates.Attacking, ActionStates.Dashing, ActionStates.ReceivingDamage, ActionStates.Jumping, ActionStates.Dead,
                ActionStates.Talking
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }
        
        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateRecovery(characterStatusLife.CurrentBatteries));
            
            if (!m_input.HasActionDown(InputAction.Button13) || characterStatusLife.CurrentBatteries <= 0) return;
            
            m_char.Velocity = Vector2.zero;
            InstantiateController.Instance.InstantiateEffect(RecoveryEffect, m_char.transform.position);
            characterStatusLife.CurrentHealth += characterStatusLife.RechargeAmount;
            if (characterStatusLife.CurrentHealth >= characterStatusLife.MaxHealth) {
                characterStatusLife.CurrentHealth = characterStatusLife.Maxhealth;
            }
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDamage( characterStatusLife.RechargeAmount, m_char.transform.position, characterStatusLife.CurrentHealth, characterStatusLife.MaxHealth, false));
            characterStatusLife.CurrentBatteries--;
        }
    }
}