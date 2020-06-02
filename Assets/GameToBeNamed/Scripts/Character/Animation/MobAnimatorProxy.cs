using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {
    public class MobAnimatorProxy : BaseAnimatorProxy {
        
        private void Start() {
            m_char2D.LocalDispatcher.Subscribe<OnReceivedAttack>(OnReceivedAttack);
        }
        
        private void OnReceivedAttack(OnReceivedAttack ev) {
            m_anim.SetTrigger("OnHit");
        }
    }
}