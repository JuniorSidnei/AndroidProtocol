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
            m_char2D.LocalDispatcher.Subscribe<OnTerminateAttack>(OnTerminateAttack);
        }

        private void OnDeath(OnDeath ev) {
            m_anim.SetTrigger("on_death");
        }

        private void FixedUpdate() {

            if (!m_hasGroundCollision) {
                return;
            }
            
            m_anim.SetBool("on_ground", m_char2D.Controller2D.collisions.below);
            m_anim.SetFloat("vel_x", Mathf.Abs(m_char2D.PositionDelta.x));
            m_anim.SetFloat("vel_y", Mathf.Abs(m_char2D.PositionDelta.y));
            
        }

        private void OnFirstAttack(OnFirstAttack ev) {
            m_anim.SetTrigger("first_attack");
        }

        private void OnReceivedAttack(OnReceivedAttack ev) {
            m_anim.SetTrigger("on_hit");
        }

        private void OnTerminateAttack(OnTerminateAttack ev) {
            m_anim.SetTrigger("terminate_attack");    
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