using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Character {

    public class OnTriggerEvent : MonoBehaviour {


        [SerializeReference, SelectImplementation(typeof(ICharacterAction))]
        private List<ICharacterAction> m_actions = new List<ICharacterAction>();
        
        [SerializeReference, SelectImplementation(typeof(IInputSource))]
        private IInputSource m_inputSource = new PlayerInput();

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Warrior")) {
                Debug.Log("adiciona action");
                other.GetComponent<Character2D>().AddAction(m_actions[0]);
            }
        }
    }
}