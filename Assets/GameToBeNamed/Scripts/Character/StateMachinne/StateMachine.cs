using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace GameToBeNamed.Character {


    [CreateAssetMenu(fileName = "GameToBeNamed/StateMachine")]
    public class StateMachine : ScriptableObject {

        private Character2D m_char;
        private CharacterState<Character2D> m_currentState, m_lastState;

        public void ChanceState(CharacterState<Character2D> nextState) {

            m_lastState = m_currentState;

            m_currentState?.Exit(m_char);

            m_currentState = nextState;
            m_currentState.Enter(m_char);
        }

        public CharacterState<Character2D> GetCurrentState() {
            return m_currentState;
        }

        public CharacterState<Character2D> GetLastState() {
            return m_lastState;
        }

        public void RevertToPreviousState() {
            ChanceState(m_lastState);
        }

        private void Update(Character2D character) {
           m_currentState.Update(character);
        }
    }
}