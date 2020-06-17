using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {

    [CreateAssetMenu(menuName = "GameToBeNamed/CharacterStatusLife")]
    public class CharacterStatusLife : ScriptableObject, ISerializationCallbackReceiver {

        [Header("Class status")]
        public int Currenthealth;
        public int Maxhealth;
        public int Money;

        [Header("Class hud settings")]
        public Sprite IconSplash;
        public Sprite LifeSplash;
        
        //Alteram valor durante o jogo
        private int m_maxHealthInGame;
        private int m_currentHealthInGame;
        private int m_moneyInGame;

        //Valores iniciais
        public void OnBeforeSerialize() {
            Currenthealth = Maxhealth;
            Money = 0;
        }

        public void OnAfterDeserialize() {

            m_maxHealthInGame = Maxhealth;
            m_currentHealthInGame = Currenthealth;
            m_moneyInGame = Money;
        }

        public int CurrentHealth {
            get => m_currentHealthInGame;
            set => m_currentHealthInGame = value;
        }

        public int MaxHealth {
            get => m_maxHealthInGame;
            set => m_maxHealthInGame = value;
        }
    }
}