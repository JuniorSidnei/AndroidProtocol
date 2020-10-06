using System;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class JumpAction : CharacterAction {

        private List<PropertyName> m_unallowedStatus;
        
        public Vector2 JumpForce;
        [SerializeField] private float m_lowJumpMultiplier; 
        [SerializeField] private float m_fallMultiplier; 
        [SerializeField] private GameObject m_downJumpEffect; 
        [SerializeField] private GameObject m_upJumpEffect; 
        [SerializeField] private Transform m_jumpEffectPosition; 
        private IInputSource m_input;
        private Character2D m_char;
        private int m_dir;
        private bool m_landJump;

        protected override void OnConfigure() {
            
            m_input = Character2D.Input;
            m_char = Character2D;

            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }

        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }
            
            m_char.Velocity += Physics2D.gravity * Time.deltaTime;
            var inputDir = m_input.HasAction(InputAction.Button3) ? -1 : m_input.HasAction(InputAction.Button2) ? 1 : 0;
            
            if (m_char.Controller2D.collisions.above || m_char.Controller2D.collisions.below) {
                m_char.ActionStates[ActionStates.Jumping] = false;
                m_char.Velocity.y = 0;
            }

            if (m_char.Controller2D.collisions.below && m_landJump) {
                InstantiateController.Instance.InstantiateEffect(m_downJumpEffect, m_jumpEffectPosition.position);
                m_landJump = false;
            }
            
            if (m_char.Velocity.y < 0) {
                m_char.Velocity += Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.deltaTime * Vector2.up;
            }
            else if (m_char.Velocity.y > 0 && !m_input.HasAction(InputAction.Button1)) {
                m_char.Velocity += Physics2D.gravity.y * (m_lowJumpMultiplier - 1) * Time.deltaTime * Vector2.up;
            }
            


            if ((m_input.HasActionDown(InputAction.Button1) || m_input.HasAction(InputAction.Button1)) && m_char.Controller2D.collisions.below) {
                
                m_char.ActionStates[ActionStates.Jumping] = true;
                InstantiateController.Instance.InstantiateEffect(m_upJumpEffect, m_jumpEffectPosition.position);
                m_landJump = true;
                
                if (inputDir == 0) {
                    m_char.Velocity =  JumpForce * Vector2.up;
                }
                else {
                    m_char.Velocity = new Vector2(inputDir * JumpForce.x, JumpForce.y);
                }
            }
        }
    }
}