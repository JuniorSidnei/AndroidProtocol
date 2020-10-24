using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character.Sounds {
    
    public class HandleEventAudio : Character2D {

        private Character2D m_char;

        [SerializeField] private AudioClip _footSteps;
        
        private void Awake() {
            
        }
    }
}