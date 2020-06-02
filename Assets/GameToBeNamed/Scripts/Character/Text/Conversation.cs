using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {

    [CreateAssetMenu(menuName = "GameToBeNamed/NPC/Conversation")]
    public class Conversation : ScriptableObject {
        
        [TextArea(3,10)]
        public List<string> Dialogs;

        private Action m_onFinishConversationCallback;
        private Action m_onlastDialogCallback;
        private Action<string> m_currentMessageCallback;

        private int m_currentDialog = 0;

        public void Initialize(Action<string> currentMessageCallback, Action onFinishDialogCallback = null, Action onLastDialogCallback = null)
        {
            m_currentDialog = 0;
            m_onFinishConversationCallback = onFinishDialogCallback;
            m_currentMessageCallback = currentMessageCallback;
            m_onlastDialogCallback = onLastDialogCallback;
            DoCurrentMessageAction();
        }

        public void Next()
        {
            if (!isEnd)
            {
                
                m_currentDialog++;

                //If now is the end
                if (isEnd)
                    m_onlastDialogCallback?.Invoke();

                DoCurrentMessageAction();
            }
            else {
                m_onFinishConversationCallback?.Invoke();
            }
        }

        public void Prior()
        {
            if (!isInitial)
            {
                m_currentDialog--;
            }

            DoCurrentMessageAction();
        }

        private void DoCurrentMessageAction()
        {
            if (m_currentMessageCallback != null && isValid)
                m_currentMessageCallback(Dialogs[m_currentDialog]);
        }

        public int GetCurrentDialog { get { return m_currentDialog; } }
        public bool isEnd { get { return m_currentDialog == Dialogs.Count - 1; } }
        public bool isInitial { get { return m_currentDialog == 0; } }
        public bool isValid { get { return Dialogs.Count > 0 && m_currentDialog < Dialogs.Count && m_currentDialog >= 0; } }
    }
}