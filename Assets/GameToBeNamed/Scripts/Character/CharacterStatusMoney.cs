using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameToBeNamed.Character {
    [CreateAssetMenu(menuName = "GameToBeNamed/CharacterStatusMoney")]
    public class CharacterStatusMoney : ScriptableObject, ISerializationCallbackReceiver {
        
        [Header("Class status")]
        public int Money;
        

        //Alteram valor durante o jogo
        private int m_moneyInGame;

        //Valores iniciais
        public void OnBeforeSerialize() {
            Money = 0;
        }

        public void OnAfterDeserialize() {
            m_moneyInGame = Money;
        }

        public int CurrentMoney {
            get => m_moneyInGame;
            set => m_moneyInGame = value;
        }
    }
}