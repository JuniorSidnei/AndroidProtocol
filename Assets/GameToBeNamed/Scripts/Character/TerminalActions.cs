using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Character {

    public class TerminalActions : MonoBehaviour {
        

        [SerializeReference, SelectImplementation(typeof(ICharacterAction))]
        private List<ICharacterAction> m_actions = new List<ICharacterAction>();
        
//        [SerializeReference, SelectImplementation(typeof(IInputSource))]
//        private IInputSource m_inputSource = new PlayerInput();
    
        //Passar o char aqui na função como publica e atribuir as ações no player
        
        public void AddActions(Character2D character2D) {
            
            Debug.Log("adiciona action");
            foreach (var t in m_actions) {
               character2D.AddAction(t); 
            }
            
            //character2D.AddAction(m_actions[0];
        }
    }
}