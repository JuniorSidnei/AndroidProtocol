using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace GameToBeNamed.Character {


    [Serializable]
    public class StateMachine {

        class CharacterState {
            
            public IState CurrentState, LastState;
        }

        public int id;
        [SerializeReference, SelectImplementation(typeof(IState))]
        private List<IState> m_states = new List<IState>();

        private Dictionary<PropertyName, IState> m_stateMap = new Dictionary<PropertyName, IState>();

        public void OnConfigure() {
        
            foreach (var t in m_states) {
                var property = new PropertyName(t.GetType().Name);
                m_stateMap[property] = t;
            }
        }
        
        public void ChangeState<T>(Character2D character, BotInput input) {
            
            var property = new PropertyName(typeof(T).Name);
            
            if (!m_stateMap.ContainsKey(property)) { 
                throw new Exception("Não contem o tipo: " + typeof(T).Name);
            }
            
            ChangeState(character, m_stateMap[property], input);
            
        }

        public void ChangeState(Character2D character, IState state, BotInput input) {
            
            var charState = DataHolder.Instance.GetDataOrCreateForCharacter<CharacterState>(character);
            
            charState.CurrentState?.Exit(character, input);
            charState.LastState = charState.CurrentState;

            charState.CurrentState = state;
            
            charState.CurrentState.Enter(character, input);
        }

        public IState GetCurrentState(Character2D character) {
            
            var charState = DataHolder.Instance.GetDataOrCreateForCharacter<CharacterState>(character);
            return charState.CurrentState;
        }

        public IState GetLastState(Character2D character) {
            var charState = DataHolder.Instance.GetDataOrCreateForCharacter<CharacterState>(character);
            return charState.LastState;
        }

        public void RevertToPreviousState(Character2D character, BotInput input) {
            var charState = DataHolder.Instance.GetDataOrCreateForCharacter<CharacterState>(character);
            ChangeState(character, charState.LastState, input);
        }

        public void OnUpdate(Character2D character, BotInput input) {
            var charState = DataHolder.Instance.GetDataOrCreateForCharacter<CharacterState>(character);
            
            charState.CurrentState.Run(character, input);
        }
    }
}