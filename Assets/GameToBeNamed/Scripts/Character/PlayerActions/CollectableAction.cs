using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    public class CollectableAction : CharacterAction {

        private Character2D m_char;
        [SerializeField] private CharacterStatusMoney m_statusMoney;
        
        protected override void OnConfigure() {

            m_char = Character2D;
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCollectableJunkPieces>(OnCollectableJunkPieces);
            //evento para diminuir o valor do dinheiro quando comprar as pots e as habilidades
        }

        private void OnCollectableJunkPieces(OnCollectableJunkPieces ev) {
            
            m_statusMoney.CurrentMoney += ev.JunkAmount;
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateCollectable(m_statusMoney.CurrentMoney));
        }
    }
}