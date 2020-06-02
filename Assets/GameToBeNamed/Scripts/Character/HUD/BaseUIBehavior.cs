using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {
    
    public class BaseUIBehavior : MonoBehaviour {

        [SerializeField] private List<GameObject> m_playingMode, m_conversationMode, m_shopMode, m_cinematicMode;
        
        
        //Active play mode
        public virtual void HandlePlayingMode() {
            
            Debug.Log("alterando para play mode");
            ActiveList(m_conversationMode, false);
            ActiveList(m_shopMode, false);
            ActiveList(m_playingMode, true);
        }
        
        //Active conversation mode
        public virtual void HandleConversationMode() {
            
            ActiveList(m_playingMode, false);
            ActiveList(m_shopMode, false);
            ActiveList(m_conversationMode, true);
        } 
        
        
        //Active shop mode
        public virtual void HandleShopMode() {
            
            ActiveList(m_conversationMode, false);
            ActiveList(m_playingMode, false);
            ActiveList(m_shopMode, true);
        } 
        
        //Active cinematic mode
        public virtual void HandleCinematicMode() {
            
            ActiveList(m_conversationMode, false);
            ActiveList(m_playingMode, false);
            ActiveList(m_shopMode, false);
            ActiveList(m_cinematicMode, true);
        }
        
        private void ActiveList(List<GameObject> list, bool active) {
            
            list.ForEach(f => f.SetActive(active));
        }
    }
}