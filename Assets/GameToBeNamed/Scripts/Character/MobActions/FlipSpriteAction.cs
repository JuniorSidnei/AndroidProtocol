using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [Serializable]
    public class FlipSpriteAction : CharacterAction {

        /// <summary>
        /// This class allows flip sprite with oscilate class
        /// </summary>

        private Character2D m_char;
        
        [SerializeField] private OscilateAction oscilateAction;
        
        protected override void OnConfigure() {

            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        protected override void OnActivate() {
            throw new NotImplementedException();
        }

        protected override void OnDeactivate() {
            throw new NotImplementedException();
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
          
            
        }
    }
}
