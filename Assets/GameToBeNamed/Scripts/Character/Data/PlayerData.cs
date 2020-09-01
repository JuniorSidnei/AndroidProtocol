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

        public bool CompareLists(List<string> list1, List<string> list2) {

            for (var i = 0; i < list1.Count; i++) {
                if (list1[i] != list2[i]) { 
                    Debug.Log("lista1: " + list1[i] + "lista2: " + list2[i]);
                    return false;
                }
            }
            Debug.Log("é verdade");
            return true;
        }
    }
}