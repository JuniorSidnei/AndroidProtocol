using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class MobProcessDamageAction : CharacterAction {

        private Character2D m_char;
        public List<PropertyName> UnallowedStatus;
        [SerializeField] private GameObject m_onHitEffect;
        [SerializeField] private Vector2 m_knockbackForce;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            
            m_char.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {

            m_char.ActionStatus[ActionStates.ReceivingDamage] = true;
            
            InstantiateController.Instance.InstantiateEffect(m_onHitEffect, ev.DamageContact);
            GameManager.Instance.GlobalDispatcher.Emit(new OnCameraScreenshake(3, .1f));

            if (ev.AttackInfo.Emiter.transform.position.x > m_char.transform.position.x) {
                m_char.Velocity = new Vector2(-m_knockbackForce.x, m_knockbackForce.y);
            }
            else {
                m_char.Velocity = new Vector2(m_knockbackForce.x, m_knockbackForce.y);
            }
            
            var to = m_char.Velocity;
            DOTween.To(() => Character2D.Velocity, x => Character2D.Velocity = x, to, .2f).SetEase(Ease.Linear);
        }
    }
}