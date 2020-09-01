using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Character.Data;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character.NPC {

    public class TerminalNpcBehavior : NpcBehavior {

        [SerializeField] private PlayerData m_playerData;

        [Header("Dialogs")]
        public Conversation IntroductionConversation;

        private Character2D m_character2D;
        
        public void Update () {
            
            if (m_player.GetButtonDown("Action") && OnConversationActive) {
                
                GameManager.Instance.GlobalDispatcher.Emit(new OnTalking(this, true));
                Run(() => {
                    Debug.Log("já terminei, voltar normal");
                    UIManager.Instance.HandlePlaying();
                    GameManager.Instance.GlobalDispatcher.Emit(new OnTalking(this, false));
                    Count = 0;
                    OnConversationActive = false;
                });
            }
            
            if (m_player.GetButtonDown("Jump") && m_npcState != NPCstates.None) {
                ChangeNPCState();
            }
            
            if (m_player.GetButtonDown("CancelAction") && m_npcState != NPCstates.None) {
                UIManager.Instance.HandlePlaying();
                GameManager.Instance.GlobalDispatcher.Emit(new OnTalking(this, false));
                Count = 0;
                OnConversationActive = false;
            }
        }
        
        protected override bool Run(Action OnFinish) {
            
            m_canvas.DOFade(0, .5f);
            Debug.Log("to na Run");
//            if (m_animator) {
//                SetTalkingCallbacks(
//                    () => { m_animator.SetBool("Speaking", true); },
//                    () => { m_animator.SetBool("Speaking", false); }
//                );
//            }

            if (!base.Run(OnFinish)) {
                if (IntroductionConversation) {
                    SetIntroduction(IntroductionConversation, NpcSprite, AddActions);
                }
            }
            
            return false;
        }

        private void AddActions() {
            m_character2D.ShiftingActionsData.SetActionList(m_playerData.Actions);
            m_character2D.UpdateActions();
            Finish();
        }
        
        
        private void OnTriggerEnter2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            m_canvas.DOFade(1, 1f);
            OnConversationActive = true;
            m_character2D = other.gameObject.GetComponent<Character2D>();
        }
        
        private void OnTriggerExit2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            m_canvas.DOFade(0, 1f);
            OnConversationActive = false;
            m_character2D = null;
        }
    }
}