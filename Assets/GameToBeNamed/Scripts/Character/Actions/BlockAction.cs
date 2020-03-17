using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{

    [System.Serializable]
    public class BlockAction : CharacterAction
    {
        public Character2D.Status UnallowedStatus;
        public BoxCollider2D Block;
        

        protected override void OnConfigure() {
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            Character2D.AnimatorProxy.OnBlockFinish.AddListener(OnDeactiveBlockBox);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {

            if (Character2D.HasStatus(UnallowedStatus) || !Character2D.Controller2D.collisions.below) {
                return;
            }
            
            if (Character2D.Input.HasActionDown(InputAction.Button5)) {
                Debug.Log("bloquenado?");
                Character2D.SetStatus(Character2D.Status.Blocking);

                Block.enabled = true;
                Debug.Log("ativei a box");
            }
            else {
                Character2D.UnsetStatus(Character2D.Status.Blocking);
            }
        }

        private void OnDeactiveBlockBox() {
           Debug.Log("recebendo evento");
            Block.enabled = false;
        }
    }
}