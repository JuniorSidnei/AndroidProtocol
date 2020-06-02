using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    
    public class ShopNpcBehavior : NpcBehavior {

        [Header("Dialogs")]
        public Conversation IntroductionConversation, SellingConversation;
        
        [SerializeField]
        protected Animator m_animator;

        public LayerMask PlayerLayer;

        [Header("Canvas issues")]
        [SerializeField]
        private CanvasGroup m_canvas;
        
        public override bool Run(Action OnFinish)
        {
            Debug.Log("to na Run");
//            if (m_animator) {
//                SetTalkingCallbacks(
//                    () => { m_animator.SetBool("Speaking", true); },
//                    () => { m_animator.SetBool("Speaking", false); }
//                );
//            }

            if (!base.Run(OnFinish)) {
                if (IntroductionConversation) {
                    SetIntroduction(IntroductionConversation, NpcSprite, OnFinishIntroduction);
                }
            }
            
            return false;
        }

        protected virtual void OnFinishIntroduction() {
            
            if (SellingConversation) {
                SetConversation(SellingConversation, NpcSprite, OnFinishPosIntro);
            }
        }

        protected virtual void OnFinishPosIntro() {
            //emitir evento da hud para abrir a loja
            UIManager.Instance.HandlePlaying();
            Finish();
        }

        private void OnTriggerEnter2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            m_canvas.DOFade(1, .5f);
            GameManager.Instance.GlobalDispatcher.Emit(new OnTalking(this, true));
            
//            Run(() => {
//                Debug.Log("já terminei, voltar normal");
//                UIManager.Instance.HandlePlaying();
//                GameManager.Instance.GlobalDispatcher.Emit(new OnTalking(this, false));
//                Count = 0;
//            });
        }
        
        private void OnTriggerExit2D(Collider2D other) {

            if (((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            UIManager.Instance.HandlePlaying();
        }
    }
}