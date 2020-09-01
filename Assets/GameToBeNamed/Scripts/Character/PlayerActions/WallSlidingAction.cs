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

        protected override void OnConfigure() {
            
            

            m_unallowedStatus = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.ReceivingDamage
            };
        }

        protected override void OnActivate() {
            Character2D.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }

        protected override void OnDeactivate() {
            Character2D.LocalDispatcher.Unsubscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }


        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev) {
            if (Character2D.ActionStates.AllNotDefault(m_unallowedStatus).Any() || Character2D.Controller2D.collisions.below) {
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