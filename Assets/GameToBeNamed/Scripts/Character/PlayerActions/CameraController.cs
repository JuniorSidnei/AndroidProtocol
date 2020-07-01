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
        [SerializeField] private float m_originalOffset;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_input = m_char.Input;
            
            m_char.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }

        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {
            
            if (m_input.HasActionDown(InputAction.Button10)) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraLookPosition(1, m_lookUp, m_lookDown, m_originalOffset));
            }
            else if (m_input.HasActionDown(InputAction.Button11)) {
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraLookPosition(2, m_lookUp, m_lookDown, m_originalOffset));
            }
            else if(m_input.HasActionUp(InputAction.Button10) || m_input.HasActionUp(InputAction.Button11)){
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraLookPosition(0, m_lookUp, m_lookDown, m_originalOffset));
            }
        }
    }
}