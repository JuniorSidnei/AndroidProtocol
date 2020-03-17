using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Character;
using UnityEngine;

namespace GameToBeNamed.Character
{
    [System.Serializable]
    public class AttackAction : CharacterAction
    {

        public Character2D.Status UnallowedStatus;
        
        public float Damage;
        public Transform AttackPoint;
        public float AttackRange = 0.5f;
        public LayerMask EnemyLayer;
        
        
        protected override void OnConfigure() {
            Character2D.LocalDispatcher.Subscribe<OnCharacterUpdate>(OnCharacterUpdate);
        }

        private void OnCharacterUpdate(OnCharacterUpdate ev) {

            if (Character2D.HasStatus(UnallowedStatus) || !Character2D.Controller2D.collisions.below) {
                return;
            }
            
            if (Character2D.Input.HasActionDown(InputAction.Button4)) {
                Debug.Log("atacando?");
                Character2D.SetStatus(Character2D.Status.Attack);

                Collider2D[] onHitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer);

                foreach (var enemy in onHitEnemies)
                {
                    //TODO eventDamage passando o inimigo
                    Debug.Log("Acertei!");
                }
            }
            else {
                Character2D.UnsetStatus(Character2D.Status.Attack);
            }
        }
    }
}