using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using GameToBeNamed.Utils;
using JetBrains.Annotations;

namespace GameToBeNamed.Character
{
    [System.Serializable]
    public class DashAction : CharacterAction {
        
        private List<PropertyName> m_unallowedStatus;
        private IInputSource m_input;
        private Character2D m_char;
        
        public GameObject DashEffect;
        [SerializeField] private Transform m_dashPositionEffect;
        [SerializeField] private float m_dashCooldown;
        public float DashForce, DashDrag;
        private int m_dir;
        private float m_dashCooldownTimer;
        
        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.ActionStates[ActionStates.Dashing] = false;
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage   
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {

           
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }
            
            if (m_input.HasAction(InputAction.Button2)) {
                m_dir = 1;
            }
            else if (m_input.HasAction(InputAction.Button3)) {
                m_dir = -1;
            }

            m_dashCooldownTimer -= Time.deltaTime;
            
            if (m_input.HasActionDown(InputAction.Button6) && m_dashCooldownTimer <= 0) {

                m_char.ActionStates[ActionStates.Dashing] = true;
                m_char.LocalDispatcher.Emit(new OnDashing());
                m_char.Velocity.x = DashForce * m_dir;
                m_char.Drag = DashDrag;
                InstantiateController.Instance.InstantiateDirectionalEffect(DashEffect, m_dashPositionEffect.position, m_dir);
                m_dashCooldownTimer = m_dashCooldown;
                var to = m_char.Velocity.x;
                DOTween.To(() => Character2D.Velocity.x, x => Character2D.Velocity.x = to, to, .2f).SetEase(Ease.Linear).OnComplete(() => {
                    m_char.ActionStates[ActionStates.Dashing] = false;
                });
            }
        }
    }
}