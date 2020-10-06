using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character.Data {

    [CreateAssetMenu(menuName = "GameToBeNamed/Data")]
    public class PlayerData : ScriptableObject {
        
        [SerializeField]
        private List<string> m_actions;

        public List<string> Actions {
            get => m_actions;
            set => m_actions = value;
        }

        public void SetActionList(List<string> action) {
            m_actions = action;
        }
    }
}