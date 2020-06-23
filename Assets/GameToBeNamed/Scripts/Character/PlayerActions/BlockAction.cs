using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{

    [System.Serializable]
    public class BlockAction : CharacterAction
    {
        private Character2D m_char;
        private IInputSource m_input;
        private List<PropertyName> m_unallowedStatus;
        public int id;

        protected override void OnConfigure() {
            m_char = Character2D;
            m_input = m_char.Input;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnBlockFinish>(OnBlockFinish);
            
            m_char.ActionStatus.Add(ActionStates.Blocking, false);
            
            m_unallowedStatus  = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage    
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {

            if (m_char.ActionStatus.AllNotDefault(m_unallowedStatus).Any() || id == 1) {
                return;
            }

            if (m_input.HasActionDown(InputAction.Button5)) {
                ActiveBlockBox();
            }
        }

        private void ActiveBlockBox() {
            m_char.ActionStatus[ActionStates.Blocking] = true;
            m_char.LocalDispatcher.Emit(new OnBlocking());
        }
        
        private void OnBlockFinish(OnBlockFinish ev) {
            m_char.ActionStatus[ActionStates.Blocking] = false;
        }
    }
}