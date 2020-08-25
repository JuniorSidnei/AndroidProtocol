using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Character;
using UnityEngine;

namespace GameToBeNamed.Utils {

    [Serializable]
    public class ProcessDamageAction : CharacterAction {

        private Character2D m_char;
        [SerializeField] private float m_knockBackForce;
        
        private int m_dir;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            
            m_char.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {
            
            m_dir = m_char.Velocity.x > 0 ? -1 : 1;
            m_char.Velocity = Vector2.zero;
            var to = m_knockBackForce * m_dir;
            DOTween.To(() => m_char.Velocity.x, x => m_char.Velocity.x = x, to, .2f).SetEase(Ease.Linear);
            
        }
    }
}