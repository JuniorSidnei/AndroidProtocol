using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class CameraController : CharacterAction {

        private IInputSource m_input;
        private Character2D m_char;

        [SerializeField] private float m_lookUp;
        [SerializeField] private float m_lookDown;
        [SerializeField] private float m_defaultOffset;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_input = m_char.Input;
            
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {

            if (m_input.HasActionDown(InputAction.Button10) || m_input.HasAction(InputAction.Button10)) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraConfigureOffset(1, m_lookUp, m_lookDown, m_defaultOffset));
            }
            else if (m_input.HasActionDown(InputAction.Button11) || m_input.HasAction(InputAction.Button11)) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraConfigureOffset(2, m_lookUp, m_lookDown, m_defaultOffset));
            }
            else if(m_input.HasActionUp(InputAction.Button10) || m_input.HasActionUp(InputAction.Button11)) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraConfigureOffset(0, m_lookUp, m_lookDown, m_defaultOffset));
            }
        }
    }
}