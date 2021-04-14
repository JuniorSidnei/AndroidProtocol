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
        public int RechargeableBatteries;
        public int RechargeAmountValue;
        
        
        [Header("Class hud settings")]
        public Sprite IconSplash;
        public Sprite LifeSplash;
        
        //Alteram valor durante o jogo
        private int m_max_health_in_game;
        private int m_current_health_in_game;
        private int m_money_in_game;
        private int m_rechargeable_batteries_in_game;
        private int m_recharge_amount_value_in_game;
        
        //Valores iniciais
        public void OnBeforeSerialize() {
            Currenthealth = Maxhealth;
            Money = 0;
            m_rechargeable_batteries_in_game = RechargeableBatteries;
            m_recharge_amount_value_in_game = RechargeAmountValue;
        }

        public void OnAfterDeserialize() {

            m_max_health_in_game = Maxhealth;
            m_current_health_in_game = Currenthealth;
            m_money_in_game = Money;
        }

        public int CurrentHealth {
            get => m_current_health_in_game;
            set => m_current_health_in_game = value;
        }

        public int MaxHealth {
            get => m_max_health_in_game;
            set => m_max_health_in_game = value;
        }

        public int CurrentBatteries {
            get => m_rechargeable_batteries_in_game;
            set => m_rechargeable_batteries_in_game = value;
        }

        public int RechargeAmount {
            get => m_recharge_amount_value_in_game;
            set => m_recharge_amount_value_in_game = value;
        }
    }
}