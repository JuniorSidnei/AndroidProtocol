using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace GameToBeNamed.Character {

    [Serializable]
    public class MobConstituctionAction : CharacterAction {


        private Character2D m_char;
        [SerializeField] private Collision2DProxy m_collisionBox;
        private int m_life;
        [SerializeField] private int m_maxLife;
        [SerializeField] private float m_damageCooldown;
        [SerializeField] private int m_damageColision;
        [SerializeField] private Image m_lifeSplashImage;
        public GameObject ExplosionEffect;
        public GameObject MoneyBox;
        private float m_damageCooldownTimer;

        protected override void OnConfigure() {
            m_char = Character2D;
            
            m_char.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
            m_collisionBox.OnCollision2DEnterCallback.AddListener(OnCollision2DEnterCallback);

            m_life = m_maxLife;
        }

        
        private void OnCollision2DEnterCallback(Collision2D ev) {
            
            var info = new OnAttackTriggerEnter.Info {
                    Emiter = m_char.gameObject, Receiver = ev.gameObject };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damageColision, ev.contacts[0].point));
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {

            if (m_damageCooldownTimer > Time.time || ev.OnBlocking) {
                return;
            }
            
            m_damageCooldownTimer = Time.time + m_damageCooldown;
            m_life -= ev.Damage;
            m_lifeSplashImage.fillAmount = (float) m_life / m_maxLife;

            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDamage(ev.Damage, m_char.transform.position,
                m_life, m_maxLife, false));
            

            if (m_life > 0) return;
            
            InstantiateController.Instance.InstantiateEffect(ExplosionEffect, m_char.transform.position);

            for (var i = 0; i < 5; i++) {
                InstantiateController.Instance.InstantiateEffect(MoneyBox, m_char.transform.position);
            }
                
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDeath(m_char));
        }
    }
}