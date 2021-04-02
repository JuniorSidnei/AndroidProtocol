using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class MobToxicGasCloudAction : CharacterAction {

        private Character2D m_char;
        [SerializeField] private Collision2DProxy m_collisionBox;
        [SerializeField] private int m_damageColision;
        [SerializeField] private float m_timeActivated;
        [SerializeField] private ParticleSystem m_particleSystem;
        [SerializeField] private AnimationCurve m_emissionRateAnimation;

        protected override void OnConfigure() {
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        protected override void OnActivate() {
            GameManager.Instance.GlobalDispatcher.Emit(new OnAddNewMobOnCombatList (m_char));
            m_collisionBox.OnCollision2DEnterCallback.AddListener(OnCollision2DEnterCallback);
        }

        protected override void OnDeactivate() {
            m_collisionBox.OnCollision2DEnterCallback.RemoveListener(OnCollision2DEnterCallback);
        }
        
        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            m_timeActivated -= Time.deltaTime;

            var particleSystemEmission = m_particleSystem.emission;
            particleSystemEmission.rateOverTime = new ParticleSystem.MinMaxCurve(m_timeActivated, m_emissionRateAnimation);
            
            if (m_timeActivated <= 0) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterDeath(m_char));
            }
        }

        private void OnCollision2DEnterCallback(Collision2D ev) {
            
            var info = new OnAttackTriggerEnter.Info {
                Emiter = m_char.gameObject, Receiver = ev.gameObject };
            GameManager.Instance.GlobalDispatcher.Emit(new OnAttackTriggerEnter(info, m_damageColision, ev.contacts[0].point));
        }
    }
}