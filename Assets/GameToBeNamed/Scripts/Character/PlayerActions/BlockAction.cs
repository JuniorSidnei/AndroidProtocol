using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{

    [System.Serializable]
    public class BlockAction : CharacterAction
    {
        private Character2D m_char;
        private List<PropertyName> m_unallowedStatus;
        [SerializeField] private BoxCollider2D Block;
        [SerializeField] private float BlockBoxPosition;
        
        
        protected override void OnConfigure() {
            m_char = Character2D;
            m_char.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
            m_char.LocalDispatcher.Subscribe<OnBlockFinish>(OnBlockFinish);
            
            m_char.ActionStatus.Add(ActionStates.Blocking, false);
            
            m_unallowedStatus  = new List<PropertyName>() {
                ActionStates.Dead, ActionStates.Talking, ActionStates.ReceivingDamage    
            };
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {

            if (m_char.ActionStatus.AllNotDefault(m_unallowedStatus).Any() || !m_char.Controller2D.collisions.below) {
                return;
            }

            OnBlocking();
            
            if (m_char.Input.HasActionDown(InputAction.Button5) && m_char.Velocity.x > 0) {//right
               ActiveBlockBox(-BlockBoxPosition);
            }
            else if(m_char.Input.HasActionDown(InputAction.Button5) &&  m_char.Velocity.x < 0){//left
                ActiveBlockBox(BlockBoxPosition);
            }
        }

        private void ActiveBlockBox(float blockPos) {

            m_char.ActionStatus[ActionStates.Blocking] = true;
            m_char.LocalDispatcher.Emit(new OnBlocking());
            Block.transform.localPosition = new Vector3(-blockPos, 0,0);
            Block.enabled = true;
        }
        
        private void OnBlockFinish(OnBlockFinish ev) {
            Block.enabled = false;
            m_char.ActionStatus[ActionStates.Blocking] = false;
        }

        private void OnBlocking() {
            if (m_char.ActionStatus.IsSet(ActionStates.Blocking)) {
                m_char.Velocity = Vector2.zero;
            }
        }
    }
}