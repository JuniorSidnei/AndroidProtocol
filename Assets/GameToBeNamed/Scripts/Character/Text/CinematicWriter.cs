using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Character {

    public class CinematicWriter : MonoBehaviour {
        [Header("Audio typing")]
        [SerializeField] private AudioClip m_textTyping;
        
        [Header("Text settings")]
        public Conversation CinematicConversation;
        public TextMeshProUGUI TextConversation;

        [Header("Buttons")]
        public GameObject NextButton;
        public GameObject SkipButton;
        
        [Header("Transition")]
        [SerializeField] private TransitionImageController m_transitionImageController;

        //TODO: FAZER TRANSIÇÃO PARA A CENA DO GAME E AJUSTAR O CALLBACK DA ULTIMA FALA OU DEIXAR FALA EM BRANCO
        private void Start() {
            m_transitionImageController.transitionOut(() => {
                CinematicConversation.Initialize((message) => {
                    this.WriteText(TextConversation, message, m_textTyping, () => { 
                        NextButton.SetActive(true);
                    });
                }, null, cutSpeedch);
            });

            StartCoroutine(showSkipButton());
        }

        public void nextDialog() {
            CinematicConversation.Next();
            NextButton.SetActive(false);
        }

        public void cutSpeedch() {
            SceneManager.LoadScene("Laboratory");
        }

        private IEnumerator showSkipButton() {
            yield return new WaitForSeconds(3f);
            SkipButton.SetActive(true);
        }
    }
}