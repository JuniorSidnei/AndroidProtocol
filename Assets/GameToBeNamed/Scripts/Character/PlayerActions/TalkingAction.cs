using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class TalkingAction : CharacterAction {

        private Character2D m_char;
        public bool OnTalkingNpc;

        protected override void OnConfigure() {
           
            m_char = Character2D;
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnTalking>(OnTalking);
        }

        private void OnTalking(OnTalking ev) {
            
            m_char.ActionStatus[ActionStates.Talking] = ev.OnTalkingNpc;
            m_char.Velocity = Vector2.zero;
            Debug.Log("falando? " +  m_char.ActionStatus[ActionStates.Talking]);
        }
    }
}