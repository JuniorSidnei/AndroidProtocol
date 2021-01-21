using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace GameToBeNamed.Character {

    [Serializable]
    public class MobConstituctionAction : CharacterAction {


        private Character2D m_char;
        [SerializeField] private Collision2DProxy m_collisionBox;
        private int m_life;
        [SerializeField] private bool _hasDeathAnimation;
        [SerializeField] private int m_maxLife;
        [SerializeField] private float m_damageCooldown;
        [SerializeField] private bool _hasDamageCollision;
        [SerializeField] private int m_damageColision;
        [SerializeField] private Image m_lifeSplashImage;
        [SerializeField] private AudioClip m_dieExplosionSound;
        [SerializeField] private AudioClip m_hurtSound;
        public GameObject ExplosionEffect;
        public GameObject MoneyBox;
        private float m_damageCooldownTimer;

        protected override void OnConfigure() {
            m_char = Character2D;
            m_life = m_maxLife;
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
            m_char.LocalDispatcher.Subscribe<OnFinishDeath>(OnFinishDeath);
            m_collisionBox.OnCollision2DEnterCallback.AddListener(OnCollision2DEnterCallback);
        }

        
        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnReceivedAttack>(OnReceivedAttack);
            m_char.LocalDispatcher.Unsubscribe<OnFinishDeath>(OnFinishDeath);
            m_collisionBox.OnCollision2DEnterCallback.RemoveListener(OnCollision2DEnterCallback);
        }


        private void OnCollision2DEnterCallback(Collision2D ev) {
            if (!_hasDamageCollision) return;
            
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char.gameObject, Receiver = ev.gameObject
            };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damageColision,
                ev.contacts[0].point));
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {

            if (m_damageCooldownTimer > Time.time || ev.OnBlocking || ev.AttackInfo.Emiter == null) {
                return;
            }
            
            AudioController.Instance.Play(m_hurtSound, AudioController.SoundType.SoundEffect2D, 0.2f);
            m_damageCooldownTimer = Time.time + m_damageCooldown;
            m_life -= ev.Damage;
            
            if(m_lifeSplashImage)
                m_lifeSplashImage.fillAmount = (float) m_life / m_maxLife;

            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDamage(ev.Damage, m_char.transform.position,
                m_life, m_maxLife, false));
            

            if (m_life > 0) return;

            
            if (_hasDeathAnimation) {
                m_char.LocalDispatcher.Emit(new OnDeath(m_char));
            }
            else {
                OnDeath();
            }
        }

        private void OnDeath() {
            
            InstantiateController.Instance.InstantiateEffect(ExplosionEffect, m_char.transform.position);
            AudioController.Instance.Play(m_dieExplosionSound, AudioController.SoundType.SoundEffect2D, 0.2f);

            for (var i = 0; i < 5; i++) {
                InstantiateController.Instance.InstantiateEffect(MoneyBox, m_char.transform.position);
            }
                
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDeath(m_char));
        }
        
        private void OnFinishDeath(OnFinishDeath ev) {
            
            for (var i = 0; i < 5; i++) {
                InstantiateController.Instance.InstantiateEffect(MoneyBox, m_char.transform.position);
            }
                
            GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDeath(m_char));
        }
    }
}