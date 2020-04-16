using UnityEngine;

namespace GameToBeNamed.Character {
    
    [System.Serializable]
    public class PlayerInput : InputSource {
        
        public int playerNumber;
        
        public override void Update() {
            CheckButtonDown("Jump", InputAction.Button1);
            CheckButton("MoveRight", InputAction.Button2);
            CheckButton("MoveLeft", InputAction.Button3);            
        }

        private void CheckButton(string buttonName, InputAction actionValue) {
            if (Input.GetButton(buttonName)) {
                SetAction(actionValue);
            }
            else {
                UnsetAction(actionValue);
            }
        }
        
        private void CheckButtonDown(string buttonName, InputAction actionValue) {
            if (Input.GetButtonDown(buttonName)) {
                SetActionDown(actionValue);
            }
            else {
                UnsetActionDown(actionValue);
            }
        }
        
        private void CheckButtonUp(string buttonName, InputAction actionValue) {
            if (Input.GetButtonUp(buttonName)) {
                SetActionUp(actionValue);
            }
            else {
                UnsetActionUp(actionValue);
            }
        }
    }
}