using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;

namespace GameToBeNamed.Character {

    public class BattleCanvas : Singleton<BattleCanvas> {

        public TextMeshProUGUI DamageText;

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterDamage>(OnCharacterDamage);
        }

        private void OnCharacterDamage(OnCharacterDamage ev) {
            
            var  damagetext = Instantiate(DamageText, ev.Position, Quaternion.identity, transform);
            damagetext.text = ev.Damage.ToString();
        }

    }
}