using UnityEngine;
using Rewired;


namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class BotInput : InputSource {
        
        [SerializeField] private StateMachine m_stateMachine;

        private Character2D m_char;
        
        public override void Configure(Character2D character) {
            m_char = character;
            m_stateMachine.OnConfigure();
            m_stateMachine.ChangeState<MechaReptileIddleState>(m_char, this);
        }

        public override void Update() {
            m_stateMachine.OnUpdate(m_char, this);
        }
    }
}