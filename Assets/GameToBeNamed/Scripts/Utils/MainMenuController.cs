using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Utils {

    public class MainMenuController : MonoBehaviour {

        [SerializeField] private TransitionImageController m_transitionImageController;
        [SerializeField] private CanvasGroup m_menuPanel;
        [SerializeField] private TextMeshProUGUI m_mainTittle;

        private void Start() {
            StartCoroutine(showMainTittle());
        }

        public void startGame() {
            m_menuPanel.DOFade(0, 0.2f).SetEase(Ease.InQuad);
            m_transitionImageController.transitionIn();
        }
        
        public void quitGame () {
            Application.Quit(0);
        }

        private IEnumerator showMainTittle() {
            yield return new WaitForSeconds(0.5f);
            m_mainTittle.DOFade(1, 0.5f).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(1f);
            m_menuPanel.DOFade(1, 1f).SetEase(Ease.InQuad);
        }
    }
}