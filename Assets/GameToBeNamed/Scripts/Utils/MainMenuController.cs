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
        [Header("Menu")]
        [SerializeField] private CanvasGroup m_menuPanel;
        [SerializeField] private TextMeshProUGUI m_mainTittle;
        [Header("Options")]
        [SerializeField] private GameObject m_optionsPanel;
        [SerializeField] private GameObject m_controllerPanel;
        [SerializeField] private GameObject m_keyboardPanel;
        [SerializeField] private GameObject m_buttonsOptionsPanel;
       

        private void Start() {
            StartCoroutine(showMainTittle());
        }

        public void startGame() {
            m_menuPanel.DOFade(0, 0.2f).SetEase(Ease.InQuad);
            m_transitionImageController.transitionIn();
        }
        
        public void quitGame() {
            Application.Quit(0);
        }

        public void options() {
            m_mainTittle.DOFade(0, 0.2f).SetEase(Ease.InQuad);
            m_menuPanel.DOFade(0, 0.2f).OnComplete(() => { m_optionsPanel.SetActive(true); });
        }

        public void showController() {
            m_buttonsOptionsPanel.SetActive(false);
            m_controllerPanel.SetActive(true);
        }
        
        public void showKeyboard() {
            m_buttonsOptionsPanel.SetActive(false);
            m_keyboardPanel.SetActive(true);
        }

        public void hidePanel(GameObject panel) {
            panel.SetActive(false);
            m_buttonsOptionsPanel.SetActive(true);
        }

        public void returnMenu() {
            m_optionsPanel.SetActive(false);
            StartCoroutine(showMainTittle());
        }
        
        private IEnumerator showMainTittle() {
            yield return new WaitForSeconds(0.5f);
            m_mainTittle.DOFade(1, 0.5f).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(1f);
            m_menuPanel.DOFade(1, 1f).SetEase(Ease.InQuad);
        }
    }
}