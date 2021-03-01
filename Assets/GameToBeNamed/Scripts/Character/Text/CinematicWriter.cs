using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Character {

    public class CinematicWriter : MonoBehaviour {
        [SerializeField]
        private AudioClip m_textTyping;
        
        public Conversation CinematicConversation;
        public TextMeshProUGUI TextConversation;
        
        [SerializeField] private TransitionImageController m_transitionImageController;

        private void Start() {
            m_transitionImageController.transitionOut(() => {
                CinematicConversation.Initialize((message) => {
                    this.WriteText(TextConversation, message, m_textTyping, () => { 
                        //change to next scene});
                    });
                });
            });
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                CinematicConversation.Next();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                //change to next scene
            }
        }
    }
}