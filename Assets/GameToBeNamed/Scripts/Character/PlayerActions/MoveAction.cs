using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class MoveAction : CharacterAction {

        private List<PropertyName> m_unallowedStatus;
        
        [SerializeField] private float Speed;
        [SerializeField] private float SpeedSprint;
        private float m_originalSpeed;
        [SerializeField] private float InAirDrag = 0.5f, InGroundDrag = 5;
        [SerializeField] private SpriteRenderer Sprite;
        
        private IInputSource m_input;
        private Character2D m_char;

        protected override void OnConfigure() {
            m_input = Character2D.Input;
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage, ActionStates.Blocking
            };
            m_originalSpeed = Speed;
        }

        private void OnCharacterChangeClass(OnCharacterChangeClass ev) {
            m_char.Velocity = ev.Velocity;
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev)
        {
            
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
            
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
            
            if (m_input.HasAction(InputAction.Button2)) {//right
                m_char.Velocity += new Vector2(Speed * Time.deltaTime, 0);
                Sprite.flipX = false;
            }
            
            if (m_input.HasAction(InputAction.Button3)) {//left
                m_char.Velocity -= new Vector2(Speed * Time.deltaTime, 0);
                Sprite.flipX = true;
            }
        }
    }
}