using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {
    public class MobAnimatorProxy : BaseAnimatorProxy {

        [SerializeField] private bool m_hasGroundCollision;
        
        private void Start() {
            m_char2D.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
            m_char2D.LocalDispatcher.Subscribe<OnFirstAttack>(OnFirstAttack);
            m_char2D.LocalDispatcher.Subscribe<OnDeath>(OnDeath);
        }

        private void OnDeath(OnDeath ev) {
            m_anim.SetTrigger("OnDeath");
        }

        private void FixedUpdate() {

            if (!m_hasGroundCollision) {
                return;
            }
            
            m_anim.SetBool("OnGround", m_char2D.Controller2D.collisions.below);
            m_anim.SetFloat("VelX", Mathf.Abs(m_char2D.PositionDelta.x));
            
        }

        private void OnFirstAttack(OnFirstAttack ev) {
            m_anim.SetTrigger("FirstAttack");
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {
            m_anim.SetTrigger("OnHit");
        }
        
        
        public void ExecuteAttack() {
            m_char2D.LocalDispatcher.Emit(new OnExecuteAttack());
        }
        
        public void FinishAttack() {
            m_char2D.LocalDispatcher.Emit(new OnSecondAttackFinish());
        }

        public void FinishDeath() {
            m_char2D.LocalDispatcher.Emit(new OnFinishDeath());
        }
    }
}