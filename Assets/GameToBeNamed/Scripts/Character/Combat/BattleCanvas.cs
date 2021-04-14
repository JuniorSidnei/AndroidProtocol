using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;

namespace GameToBeNamed.Character {

    public class BattleCanvas : Singleton<BattleCanvas> {

        public TextMeshProUGUI DamageText;
        public TextMeshProUGUI RecoveryText;

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterDamage>(OnCharacterDamage);
        }

        private void OnCharacterDamage(OnCharacterDamage ev) {

            if (ev.IsDamage) {
                var damage = Instantiate(DamageText, ev.Position, Quaternion.identity, transform);
                damage.text = ev.Damage.ToString();
            }
            else {
                var recovery = Instantiate(RecoveryText, ev.Position, Quaternion.identity, transform);
                recovery.text = ev.Damage.ToString();
            }
        }
    }
}