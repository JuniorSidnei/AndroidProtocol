using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class TransitionAction : CharacterAction {

        [SerializeField] private SpriteRenderer m_rend;
        [SerializeField] private AudioClip m_changeClassSound;
        private Character2D m_char;
        private List<PropertyName> m_unallowedStatus;
        
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_rend.material.SetFloat("_Fade", 0);
            DoTransitionIn();
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage, ActionStates.Blocking, ActionStates.Dashing, ActionStates.Unconscious
            };
        }

        protected override void OnActivate() {
            m_char.LocalDispatcher.Subscribe<OnCharacterTransition>(OnCharacterTransition);
        }

        protected override void OnDeactivate() {
            m_char.LocalDispatcher.Unsubscribe<OnCharacterTransition>(OnCharacterTransition);
        }

        private void OnCharacterTransition(OnCharacterTransition ev) {
            if (m_char.ActionStates.AllNotDefault(m_unallowedStatus).Any()) {
                return;
            }

            DoTransitionOut(ev.OnTransitionCallBack);
        }
        
        
        private void DoTransitionIn() {
            m_rend.material.DOFloat(1, "_Fade", .5f);
            AudioController.Instance.Play(m_changeClassSound, AudioController.SoundType.SoundEffect2D, 0.5f);
        }
        
        private void DoTransitionOut(Action onTransitionCallBack) {
            m_rend.material.DOFloat(0, "_Fade", .5f).OnComplete(() => {
                onTransitionCallBack?.Invoke();
            });
        }
    }
}