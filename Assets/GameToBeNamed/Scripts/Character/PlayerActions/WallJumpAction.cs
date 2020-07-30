using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{
    [System.Serializable]
    public class WallJumpAction : CharacterAction {
        
        private enum JumpType {
            None, Climbing, Leap, JumpOff
        }
        
        
        private IInputSource m_input;
        public List<PropertyName> UnallowedStatus;
        
        public Vector2 WallClimbing;
        public Vector2 WallLeap;
        public Vector2 WallJumpOff;
        
        
        protected override void OnConfigure() {
            m_input = Character2D.Input;
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {
            
            if (!Character2D.Controller2D.collisions.left && !Character2D.Controller2D.collisions.right) {
                return;
            }
            
            if (Character2D.ActionStates.AllNotDefault(UnallowedStatus).Any()) {
                return;
            }

            if(m_input.HasActionDown(InputAction.Button1)) {
             
                var inputDir = m_input.HasAction(InputAction.Button3) ? -1 : m_input.HasAction(InputAction.Button2) ? 1 : 0;
                JumpType jumpType = JumpType.None;
                float jumpDir = 0; 
                
                if (Character2D.Controller2D.collisions.left){
                    jumpDir = 1;
                    if (inputDir == -1) {
                        jumpType = JumpType.Climbing;
                    }
                    else if(inputDir == 1) {
                        jumpType = JumpType.Leap;
                    }
                    else {
                        jumpType = JumpType.JumpOff;
                    }
                }
                
                if (Character2D.Controller2D.collisions.right){
                    jumpDir = -1;
                    if (inputDir == 1) {
                        jumpType = JumpType.Climbing;
                    }
                    else if(inputDir == -1) {
                        jumpType = JumpType.Leap;
                    }
                    else {
                        jumpType = JumpType.JumpOff;
                    }
                }
                
                Jump(jumpType, jumpDir);
            }
        }

        private void Jump(JumpType type, float dir) {

            switch (type)
            {
                case JumpType.Climbing:
                    Character2D.Velocity.x = dir * WallClimbing.x;
                    Character2D.Velocity.y = WallClimbing.y;
                    break;
                case JumpType.Leap:
                    Character2D.Velocity.x = dir * WallLeap.x;
                    Character2D.Velocity.y = WallLeap.y;
                    break;
                case JumpType.JumpOff:
                    Character2D.Velocity.x = dir * WallJumpOff.x;
                    Character2D.Velocity.y = WallJumpOff.y;
                    break;
            }
        }
    }
}