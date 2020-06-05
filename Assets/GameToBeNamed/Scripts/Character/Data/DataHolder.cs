using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    public class DataHolder : Singleton<DataHolder> {
        
        private  Dictionary<Character2D, Dictionary<PropertyName, object>> m_characterDataMap =
            new Dictionary<Character2D, Dictionary<PropertyName, object>>();

        public  T GetDataOrCreateForCharacter<T>(Character2D character) where T : new() {
            if (!m_characterDataMap.ContainsKey(character)) {
                m_characterDataMap[character] = new Dictionary<PropertyName, object>();
            }

            var characterData = m_characterDataMap[character];
            PropertyName property = new PropertyName(typeof(T).Name);
            if (!characterData.ContainsKey(property)) {
                characterData[property] = new T();
            }

            return (T) characterData[property];
        }

        public  T GetDataForCharacter<T>(Character2D character) {
            if (!m_characterDataMap.ContainsKey(character)) {
                m_characterDataMap[character] = new Dictionary<PropertyName, object>();
            }

            var characterData = m_characterDataMap[character];
            PropertyName property = new PropertyName(typeof(T).Name);
            if (!characterData.ContainsKey(property)) {
                return default(T);
            }

            return (T) characterData[property];
        }

        public  void SetDataForCharacter<T>(Character2D character, T data) {
            if (!m_characterDataMap.ContainsKey(character)) {
                m_characterDataMap[character] = new Dictionary<PropertyName, object>();
            }

            var characterData = m_characterDataMap[character];
            PropertyName property = new PropertyName(typeof(T).Name);
            characterData[property] = data;
        }
    }
}