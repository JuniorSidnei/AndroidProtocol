using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameToBeNamed.Character {

    public class UIManager : Singleton<UIManager> {

        [SerializeField]
        private PanelPlayingBehaviour m_playingBehaviour;
        [SerializeField]
        private PanelConversationBehaviour m_conversationBehaviour;
        [SerializeField]
        private PanelShopBehaviour m_shopBehaviour;


        public void HandlePlaying() {
            m_playingBehaviour.HandlePlayingMode();
            m_conversationBehaviour.HandlePlayingMode();
            m_shopBehaviour.HandlePlayingMode();
        }

        public void HandleIntroductionConversation(string npcnName, string conversation, Sprite dialogSprite, Action onFinish) {
            m_conversationBehaviour.HandleConversationMode();
            m_conversationBehaviour.SetIntroductionConversation(npcnName, conversation, dialogSprite, onFinish);
            m_playingBehaviour.HandleConversationMode();
            m_shopBehaviour.HandleConversationMode();
        }
        
        
        public void HandleConversation(string npcName, string conversation, Sprite dialogSprite, Action onFinish) {
            m_conversationBehaviour.HandleConversationMode();
            m_conversationBehaviour.SetConversation(npcName, conversation, dialogSprite, onFinish);
            m_playingBehaviour.HandleConversationMode();
            m_shopBehaviour.HandleConversationMode();
        }

        public void HandleShop() {
            m_shopBehaviour.HandleShopMode();
            m_shopBehaviour.SetShop();
            m_conversationBehaviour.HandleShopMode();
            m_playingBehaviour.HandleShopMode();
        }
        
        public bool IsTyping() {
            return m_conversationBehaviour.IsTyping();
        }
        
        public static void Show() {
            SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
        }
        
    }
}
