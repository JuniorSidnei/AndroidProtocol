using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Utils {

    public class TransitionImageController : MonoBehaviour {
        
        private Image m_image;

        private void Awake() {
            m_image = GetComponent<Image>();
        }

        public void transitionIn() {
            m_image.material.DOFloat(1, "_Fade", 1.5f).SetEase(Ease.InQuad).OnComplete(() => {
                changeScene("IntroductionScene");
            });
        }

        public void transitionOut(Action onTransitionFinish = null) {
            m_image.material.DOFloat(0, "_Fade", 1.5f).SetEase(Ease.InQuad).OnComplete(() => {
                onTransitionFinish?.Invoke();
            });
        }

        private void changeScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
            transitionOut();
        }
    }
}