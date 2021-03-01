using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Utils {

    public class MainMenuController : MonoBehaviour {

        [SerializeField] private TransitionImageController m_transitionImageController;
        [SerializeField] private CanvasGroup m_menuPanel;
        
        public void startGame() {
            m_menuPanel.DOFade(0, 0.2f).SetEase(Ease.InQuad);
            m_transitionImageController.transitionIn();
        }
        
        public void quitGame () {
            Application.Quit(0);
        }
    }
}