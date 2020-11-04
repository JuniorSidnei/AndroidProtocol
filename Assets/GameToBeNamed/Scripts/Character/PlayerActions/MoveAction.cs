using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class MoveAction : CharacterAction {

        private List<PropertyName> m_unallowedStatus;
        
        [SerializeField] private float Speed;
        [SerializeField] private float SpeedSprint;
        [SerializeField] private float SoundDisplacement;
        [SerializeField] private float InAirDrag = 0.5f, InGroundDrag = 5;
        [SerializeField] private SpriteRenderer m_spriteVfx;
        [SerializeField] private AudioClip m_footStepsSound;
        
        private IInputSource m_input;
        private Character2D m_char;
        private float m_soundDisplacement;
        private float m_originalSpeed;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage, ActionStates.Blocking, ActionStates.Dashing
            };
            m_originalSpeed = Speed;
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
            GameManager.Instance.GlobalDispatcher.Unsubscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
        }

        private void OnCharacterChangeClass(OnCharacterChangeClass ev) {
            m_char.Velocity = ev.Velocity;
        }

        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }
            
            
            if (m_char.Controller2D.collisions.below) {
                m_char.Drag = InGroundDrag;
            }
            else if (!m_char.Controller2D.collisions.below) {
                m_char.Drag = InAirDrag;
            }

            if (m_input.HasActionDown(InputAction.Button12)) {
                Speed = SpeedSprint;
            }
            else if(!m_input.HasActionDown(InputAction.Button12)) {
                Speed = m_originalSpeed;
            }
            
            m_soundDisplacement += Mathf.Abs(m_char.PositionDelta.x);
            
            if (m_soundDisplacement > SoundDisplacement) {
                AudioController.Instance.Play(m_footStepsSound, AudioController.SoundType.SoundEffect2D, 0.05f);
                m_soundDisplacement = 0;
            }
            
            if (m_input.HasAction(InputAction.Button2)) {//right
                m_char.Velocity += new Vector2(Speed * Time.deltaTime, 0);
                m_spriteVfx.flipX = false;
            }
            
            if (m_input.HasAction(InputAction.Button3)) {//left
                m_char.Velocity -= new Vector2(Speed * Time.deltaTime, 0);
                m_spriteVfx.flipX = true;
            }
        }
    }
}