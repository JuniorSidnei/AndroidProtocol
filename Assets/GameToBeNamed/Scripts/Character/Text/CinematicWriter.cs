using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameToBeNamed.Character {

    public class CinematicWriter : MonoBehaviour {
        [Header("Audio typing")]
        [SerializeField] private AudioClip m_textTyping;

        [Header("Cinematic stuff")]
        public List<GameObject> CinematicObjects;
        public Conversation CinematicConversation;
        public TextMeshProUGUI TextConversation;
        
        [Header("LoadingStuff")]
        public CanvasGroup LoadingStuff;
        public Image LoadingBar;
        
        [Header("Buttons")]
        public GameObject NextButton;
        public GameObject SkipButton;
        
        [Header("Transition")]
        [SerializeField] private TransitionImageController m_transitionImageController;

        private AsyncOperation sceneLoading = new AsyncOperation();
        private float loadingProgress = 0;
        
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

            foreach (var obj in CinematicObjects) {
                obj.SetActive(false);
            }
            
            LoadingStuff.DOFade(1, 0.01f);
            sceneLoading = SceneManager.LoadSceneAsync("Laboratory");
            StartCoroutine(LoadingInitialScene());
        }

        private IEnumerator showSkipButton() {
            yield return new WaitForSeconds(5f);
            SkipButton.SetActive(true);
        }

        private IEnumerator LoadingInitialScene() {
            while (!sceneLoading.isDone) {

                loadingProgress = sceneLoading.progress;

                loadingProgress = Mathf.Clamp01(sceneLoading.progress / .9f);
                LoadingBar.DOFillAmount(loadingProgress, 0.1f).SetEase(Ease.InQuad);
                yield return null;
            }
        }
    }
}