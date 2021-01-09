using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameToBeNamed.Character {

    public class CombatController : MonoBehaviour {

        [Header("Layers")]
        public LayerMask MobLayer;
        public LayerMask PlayerLayer;


        private Character2D m_char;
        [SerializeField] private List<Character2D> m_mobs;

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnAddNewMobOnCombatList>(OnAddNewMobOnCombatList);
        }

        private void OnAddNewMobOnCombatList(OnAddNewMobOnCombatList ev) {
            if (!m_char) {
                return;
            }

            ev.NewMob.transform.SetParent(transform);
            m_mobs.Add(ev.NewMob);
        }

        private void OnAttackTriggerEnter(OnAttackTriggerEnter ev) {
            
            if(ev.AttackInfo.Receiver.layer == ev.AttackInfo.Emiter.gameObject.layer) {
               return; 
            }
            
            if (((1 << ev.AttackInfo.Receiver.layer) & MobLayer) != 0) { //mob
                var attackedMob =  m_mobs.Find(mob => mob.gameObject == ev.AttackInfo.Receiver);
                attackedMob.LocalDispatcher.Emit(new OnReceivedAttack(ev.Damage, ev.DamageContact, ev.AttackInfo));
            }
            else if(((1 << ev.AttackInfo.Receiver.layer) & PlayerLayer) != 0) {//player
                GameManager.Instance.GlobalDispatcher.Emit(new OnCameraScreenshake(8, .2f));
                m_char.LocalDispatcher.Emit(new OnReceivedAttack(ev.Damage, ev.DamageContact, ev.AttackInfo));
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
           
            m_char = other.GetComponent<Character2D>();
            
            GameManager.Instance.GlobalDispatcher.Subscribe<OnAttackTriggerEnter>(OnAttackTriggerEnter);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterDeath>(OnCharacterDeath);
        }
        
        private void OnTriggerStay2D(Collider2D other) {
            
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            m_char = other.GetComponent<Character2D>();
        }

        
        private void OnCharacterDeath(OnCharacterDeath ev) {
            
            if(((1 << ev.Character.gameObject.layer) & PlayerLayer) != 0) {//player
               
                ev.OnDeathCallback?.Invoke();
                GameManager.Instance.GlobalDispatcher.Unsubscribe<OnAttackTriggerEnter>(OnAttackTriggerEnter);
                SceneManager.LoadScene("Game");
            }
            else if (((1 << ev.Character.gameObject.layer) & MobLayer) != 0) { //mob
                m_mobs.Remove(ev.Character);
                Destroy(ev.Character.gameObject);
            }
        }

        
        private void OnTriggerExit2D(Collider2D other) {
            
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            GameManager.Instance.GlobalDispatcher.Unsubscribe<OnAttackTriggerEnter>(OnAttackTriggerEnter);
            GameManager.Instance.GlobalDispatcher.Unsubscribe<OnCharacterDeath>(OnCharacterDeath);
        }
    }
}