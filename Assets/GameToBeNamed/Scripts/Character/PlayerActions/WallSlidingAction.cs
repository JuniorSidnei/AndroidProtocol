using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{
    [System.Serializable]
    public class WallSlidingAction : CharacterAction
    {
        private List<PropertyName> m_unallowedStatus;
        public float MaxWallSlideSpeed;
        private bool m_isActionCollected;
        
        protected override void OnConfigure() {
            
            Character2D.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnAddWallJumpAction>(OnAddWallJumpAction);
            
            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.ReceivingDamage
            };
        }

        private void OnAddWallJumpAction(OnAddWallJumpAction ev) {
            m_isActionCollected = true;
        }
        
        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {
            if (Character2D.ActionStates.AllNotDefault(m_unallowedStatus).Any() || Character2D.Controller2D.collisions.below || !m_isActionCollected) {
                return;
            }

            if ((Character2D.Controller2D.collisions.left || Character2D.Controller2D.collisions.right) &&
                !Character2D.Controller2D.collisions.below && Character2D.Velocity.y < 0) {
                
                Character2D.LocalDispatcher.Emit(new OnWallSliding(true));
                if (Character2D.Velocity.y < -MaxWallSlideSpeed) {
                    Character2D.Velocity.y = -MaxWallSlideSpeed;
                } 
            }
            else {
                Character2D.LocalDispatcher.Emit(new OnWallSliding(false));
            }
        }
    }
}