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
        public List<PropertyName> UnallowedStatus;
        public float MaxWallSlideSpeed;

        protected override void OnConfigure() {
            
            Character2D.LocalDispatcher.Subscribe<OnCharacterFixedUpdate>(OnCharacterFixedUpdate);
        }

        private void OnCharacterFixedUpdate(OnCharacterFixedUpdate ev)
        {
            if (Character2D.ActionStatus.AllNotDefault(UnallowedStatus).Any() || Character2D.Controller2D.collisions.below) {
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