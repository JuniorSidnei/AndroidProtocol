using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    [Serializable]
    public class HudClassConfigurationAction : CharacterAction {

        private Character2D m_char;
        [SerializeField] private Sprite m_iconSplash;
        [SerializeField] private Sprite m_lifeSplash;
        
        
        protected override void OnConfigure() {
            
            m_char = Character2D;
            
            //GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterConfigureHudClass());
        }
    }
}