using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character.Data {

    [CreateAssetMenu(menuName = "GameToBeNamed/Data")]
    public class PlayerData : ScriptableObject {
        
        [SerializeReference, SelectImplementation(typeof(ICharacterAction))]
        private List<ICharacterAction> m_actions = new List<ICharacterAction>();

        public void SetActionList(List<ICharacterAction> actionsList) {
            m_actions = actionsList;
        }
    }
}