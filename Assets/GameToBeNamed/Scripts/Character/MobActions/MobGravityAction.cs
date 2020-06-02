using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [Serializable]
    public class MobGravityAction : CharacterAction {
        
        private Character2D m_char;
        public int BotID;
        protected override void OnConfigure() {
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }


        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {
            
            m_char.Velocity += Physics2D.gravity * Time.deltaTime;

            if (m_char.Controller2D.collisions.above || m_char.Controller2D.collisions.below) {
               
                m_char.Velocity.y = 0;
            }
        }
    }
}
