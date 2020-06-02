using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameToBeNamed.Character {


    public class PanelConversationBehaviour : BaseUIBehavior {


        [SerializeField] private TextMeshProUGUI m_nameText, m_conversationText;
            
        [SerializeField]
        private Image m_npcDialogImage;
        [SerializeField]
        private AudioClip m_textTyping;
        
        private Coroutine m_textAnimationRoutine;
        
        
        public  override void HandlePlayingMode() {
            base.HandlePlayingMode();
        }
        
        public  override void HandleConversationMode() {
            base.HandleConversationMode();
        }
        
        public  override void HandleShopMode() {
            base.HandleShopMode();
        }
        
        public  override void HandleCinematicMode() {
            base.HandleCinematicMode();
        }
        
        

        public void SetIntroductionConversation(string name, string conversation, Sprite dialogSprite, Action onFinish) {

            Debug.Log("To no panel");
            m_npcDialogImage.sprite = dialogSprite;
            m_nameText.text = name;
            ChangeText(m_conversationText, conversation, onFinish);
        }

        public void SetConversation(string name, string conversation, Sprite dialogSprite, Action onFinish) {
            
            m_npcDialogImage.sprite = dialogSprite;
            m_nameText.text = name;
            ChangeText(m_conversationText, conversation, onFinish);
        }
        
        private void ChangeText(TextMeshProUGUI text, string finalText, Action onFinish) {
            
            if (m_textAnimationRoutine != null) {
                StopCoroutine(m_textAnimationRoutine);
            }
            m_textAnimationRoutine = this.WriteText(text, finalText, m_textTyping, onFinish);
        }
        
        public bool IsTyping()
        {
            if (TextWriter.Typing)
            {
                TextWriter.Complete = true;
                return true;
            }
            
            return false;
        }
    }
}