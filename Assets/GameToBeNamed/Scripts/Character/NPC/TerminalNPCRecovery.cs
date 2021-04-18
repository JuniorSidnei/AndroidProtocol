using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using UnityEngine;


namespace GameToBeNamed.Character.NPC {
    public class TerminalNPCRecovery : MonoBehaviour {

        public LayerMask PlayerLayer;
        public CanvasGroup m_canvas;

        public CharacterStatusLife m_warrior;
        public CharacterStatusLife m_shooter;

        public CharacterStatusMoney m_characterMoney;
        
        private Character2D m_character2D;

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnNpcRecovery>(OnNpcRecovery);
        }

        private void OnNpcRecovery(OnNpcRecovery ev) {
            if (!m_character2D || m_characterMoney.CurrentMoney <= 0) return;
            
            m_warrior.CurrentBatteries = m_warrior.RechargeableBatteries;
            m_shooter.CurrentBatteries = m_warrior.RechargeableBatteries;
            GameManager.Instance.GlobalDispatcher.Emit(new OnRecoveryFull( m_shooter.CurrentBatteries = m_warrior.RechargeableBatteries ));
        }
        
        private void OnTriggerEnter2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }

            m_canvas.DOFade(1, 1f);
            m_character2D = other.gameObject.GetComponent<Character2D>();
        }

        private void OnTriggerExit2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }

            m_canvas.DOFade(0, 1f);
            m_character2D = null;
        }
    }
}