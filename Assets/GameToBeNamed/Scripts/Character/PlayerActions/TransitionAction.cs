using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class TransitionAction : CharacterAction {

        [SerializeField] private SpriteRenderer m_rend;
        private Character2D m_char;
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_rend.material.SetFloat("_Fade", 0);
            DoTransitionIn();
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterTransition>(OnCharacterTransition);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterTransition>(OnCharacterTransition);
        }

        private void OnCharacterTransition(OnCharacterTransition ev) {
            DoTransitionOut(ev.OnTransitionCallBack);
        }
        
        
        private void DoTransitionIn() {
            m_rend.material.DOFloat(1, "_Fade", .5f);
        }
        
        private void DoTransitionOut(Action onTransitionCallBack) {
            m_rend.material.DOFloat(0, "_Fade", .5f).OnComplete(() => {
                onTransitionCallBack?.Invoke();
            });
        }
    }
}