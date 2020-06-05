using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using Rewired;
using UnityEngine;
using InputAction = GameToBeNamed.Character.InputAction;

public class NpcBehavior : MonoBehaviour
{
    public enum NPCstates
    {
        None,
        Introduction,
        Conversation,
        ShowShop
    }

    public string Name;

    public Sprite NpcSprite;
    [SerializeField]
    private AudioClip m_npcMusic;

    //private Action<bool> m_onAswerCallback;
    protected Action m_onFinish;
    protected NPCstates m_npcState;
    private Conversation m_currentConversation;
    private Action m_finishTalkingCallback, m_startTalkingCallback;

    [field: SerializeField]
    protected int Count { get; set; }

    [field: SerializeField] protected bool OnConversationActive { get; set; }
    

    //Player input
    public int PlayerID = 0;
    protected Player m_player;

    private void Awake() {
        m_player = ReInput.players.GetPlayer(PlayerID);
    }

    public virtual bool Run(Action onFinish) {
        
        m_onFinish = onFinish;
        //TODO ajeitar audio controller para sons
        //AudioController.Instance.Play(m_npcMusic, AudioController.SoundType.Music, 1, false, true, 0, 1, 0, 0, null, false);
            
        Count++;

        return Count >= 2;
    }

    public void SetTalkingCallbacks(Action onStartTalking, Action onFinishTalking) {
        m_startTalkingCallback = onStartTalking;
        m_finishTalkingCallback = onFinishTalking;
    }
    
    public void SetIntroduction(Conversation introduction, Sprite npcImage, Action onFinishIntroduction) {
        
        m_npcState = NPCstates.Introduction;
        m_currentConversation = introduction;
        introduction.Initialize((message) => { 
                
                m_startTalkingCallback?.Invoke();
                UIManager.Instance.HandleIntroductionConversation(Name, message, npcImage, m_finishTalkingCallback);
            },
            () => {
                CallOnFinishAndReset(onFinishIntroduction);
            }, null);
    }
    
    public void SetConversation(Conversation conversation, Sprite npcImage, Action onFinishConversation) {
        
        m_npcState = NPCstates.Conversation;
        m_currentConversation = conversation;
        conversation.Initialize((message) => {
                m_startTalkingCallback?.Invoke();

                UIManager.Instance.HandleConversation(Name, message, npcImage, m_finishTalkingCallback);
            },
            () => {
                CallOnFinishAndReset(onFinishConversation);
            }, null);
    }
    
    public void SetShopConversation(Conversation conversation, Sprite npcImage, Action onFinishConversation) {
        
        m_npcState = NPCstates.Conversation;
        m_currentConversation = conversation;
        conversation.Initialize((message) => {
                if (m_startTalkingCallback != null) {
                    m_startTalkingCallback();
                }
                //TODO ajeitar a hud para conversa da loja
                //HudManager.Instance.HandleConversation(message, npcImage, m_finishTalkingCallback);
            },
            () => {
                CallOnFinishAndReset(onFinishConversation);
            }, null);
    }
    
    public virtual void Finish() {
        m_npcState = NPCstates.None;
        m_onFinish?.Invoke();
    }
    
    
    //Os estados do npc
    public  void ChangeNPCState() {
        
        if(UIManager.Instance.IsTyping())
            return;
            
        switch (m_npcState) {
            case NPCstates.None:
                break;
            case NPCstates.Introduction:
                m_currentConversation.Next();
                break;
            case NPCstates.Conversation:
                m_currentConversation.Next();
                break;
            default:
                break;       
        }
    }

    private void CallOnFinishAndReset(Action callback) {
        m_npcState = NPCstates.None;

        callback?.Invoke();
    }
    
    //mudar essa função de update para receber o evento da hud de proxima ação para o npc
    public virtual void Update() {

//        if (m_player.GetButtonDown("Action") && OnConversationActive) {
//            
//        }
//        
//        if (m_player.GetButtonDown("Jump") && m_npcState != NPCstates.None) {
//            ChangeNPCState();
//        }
        
    }
}
