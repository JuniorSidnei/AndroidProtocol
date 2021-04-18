using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameToBeNamed.Character.NPC {
    public class TerminalNpcCanvas : MonoBehaviour {

        public LayerMask PlayerLayer;
        public CanvasGroup m_canvas;

        private void OnTriggerEnter2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }

            m_canvas.DOFade(1, 1f);
        }

        private void OnTriggerExit2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }

            m_canvas.DOFade(0, 1f);
        }
    }
}