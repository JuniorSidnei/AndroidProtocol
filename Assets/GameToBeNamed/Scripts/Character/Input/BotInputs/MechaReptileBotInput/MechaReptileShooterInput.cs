using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class MechaReptileShooterInput : BotInput {
        
        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private StateMachine m_stateMachine;
        [SerializeField] private List<Vector3> m_wayPoints;
        [SerializeField] private Collision2DProxy m_triggerProxy;
        [SerializeField] private SpriteRenderer m_spriteVfx;

        public Character2D Character { get; private set; }

        private Vector3 currentWayPoint;
        private int currentWayPointIndex;
        private GameObject m_target;

        public override void Configure(Character2D character) {
            Character = character;
            m_stateMachine.OnConfigure();
            m_stateMachine.ChangeState<PatrolState>(Character, this);
            m_triggerProxy.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
        }

        public override void Update() {

            if (m_target && Vector3.Distance(m_target.transform.position, Character.transform.position) > 40f) {
                UnsetActionDown(InputAction.Button4);
                SetTarget(null); 
                m_stateMachine.ChangeState<PatrolState>(Character, this);
            }
            
            m_stateMachine.OnUpdate(Character, this);
            
            if (Character.Controller2D.collisions.left || Character.Controller2D.collisions.right) {
                SetActionDown(InputAction.Button1);
            }
            else if (!Character.Controller2D.collisions.left || !Character.Controller2D.collisions.right) {
                UnsetActionDown(InputAction.Button1);
            }
        }
        
        public override void SetTarget(GameObject target) {
            m_target = target;
        }

        public override Vector3 GetDestinationPosition() {
            return m_target ? m_target.transform.position : currentWayPoint;
        }

        public override bool SearchDestination() { throw new System.NotImplementedException(); }
        
        public override void MoveToDestination(Vector3 destination) {
            
            SetActionDown(InputAction.Button12);
            
            if (Character.transform.position.x < destination.x) {
                UnsetAction(InputAction.Button3);
                SetAction(InputAction.Button2);
            }
            else if (Character.transform.position.x > destination.x) {
                UnsetAction(InputAction.Button2);
                SetAction(InputAction.Button3);
            }
        }

        public override void SetInitialDestination() {
            currentWayPointIndex = 0;
            currentWayPoint = m_wayPoints[currentWayPointIndex];
        }

        public override bool IsDestinationReached(Vector3 target) {
            return Vector3.Distance(Character.transform.position, target) < 1.5f;
        }

        public override bool IsTargetClose(Vector3 target) {
            if (!IsTargetSet()) return false;
            
            return Vector3.Distance(Character.transform.position, target) < 30f;
        }

        public override bool IsTargetSet() {
            return m_target;
        }

        public override void SetNextMovement() {
            currentWayPoint = m_wayPoints[++currentWayPointIndex % m_wayPoints.Count];
        }

        public override void SetRunMovement() {
            UnsetActionDown(InputAction.Button4);
            m_stateMachine.ChangeState<RunState>(Character, this);
        }

        public override void SetAttackAction() {
            if (!m_target) {
                return;
            }
            
//            if (Character.transform.position.y - 30 > m_target.transform.position.y ||
//                Character.transform.position.y + 30 < m_target.transform.position.y) {
//                return;
//            }
            
            UnsetAction(InputAction.Button12);
            m_spriteVfx.flipX = !(m_target.transform.position.x > Character.transform.position.x);

            UnsetAction(InputAction.Button2);
            UnsetAction(InputAction.Button3);
            SetActionDown(InputAction.Button4);
        }

        private void OnTrigger2DEnterCallback(Collider2D ev) {
            if (((1 << ev.gameObject.layer) & m_targetLayer) == 0 || IsTargetSet()) return;
            
            SetTarget(ev.gameObject);
            m_stateMachine.ChangeState<AttackState>(Character, this);
        }
    }
}