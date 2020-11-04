using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine;

namespace GameToBeNamed.Character {

    [Serializable]
    public class CollectableAction : CharacterAction {

        private Character2D m_char;
        [SerializeField] private CharacterStatusMoney m_statusMoney;
        [SerializeField] private AudioClip m_onCollectMoney;
        
        protected override void OnConfigure() {
            m_char = Character2D;
        }

        protected override void OnActivate() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCollectableJunkPieces>(OnCollectableJunkPieces);
        }

        protected override void OnDeactivate() {
            GameManager.Instance.GlobalDispatcher.Unsubscribe<OnCollectableJunkPieces>(OnCollectableJunkPieces);
        }

        private void OnCollectableJunkPieces(OnCollectableJunkPieces ev) {
            
            AudioController.Instance.Play(m_onCollectMoney, AudioController.SoundType.SoundEffect2D, 0.08f);
            m_statusMoney.CurrentMoney += ev.JunkAmount;
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateCollectable(m_statusMoney.CurrentMoney));
        }
    }
}