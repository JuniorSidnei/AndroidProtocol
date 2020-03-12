using UnityEngine;
using Rewired;


namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class PlayerInput : InputSource {

        public int PlayerID = 0;
        private Player m_player;

        public PlayerInput() {
            m_player = ReInput.players.GetPlayer(PlayerID);
        }
        
        
        public override void Update() {
            CheckButtonDown("Jump", InputAction.Button1);
            CheckButton("MoveRight", InputAction.Button2);
            CheckButton("MoveLeft", InputAction.Button3);           
        }

        
        private void CheckButton(string buttonName, InputAction actionValue) {
            if (m_player.GetButton(buttonName)) {
                SetAction(actionValue);
            }
            else {
                UnsetAction(actionValue);
            }
        }
        
        private void CheckButtonDown(string buttonName, InputAction actionValue) {
            if (m_player.GetButtonDown(buttonName)) {
                SetActionDown(actionValue);
            }
            else {
                UnsetActionDown(actionValue);
            }
        }
        
        private void CheckButtonUp(string buttonName, InputAction actionValue) {
            if (m_player.GetButtonUp(buttonName)) {
                SetActionUp(actionValue);
            }
            else {
                UnsetActionUp(actionValue);
            }
        }
    }
}