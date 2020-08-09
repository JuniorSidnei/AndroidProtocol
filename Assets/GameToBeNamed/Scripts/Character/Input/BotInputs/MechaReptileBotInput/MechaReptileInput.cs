using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class MechaReptileInput : BotInput {
        
        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private float m_attackSpeed = 2f;
        [SerializeField] private StateMachine m_stateMachine;
        [SerializeField] private List<Vector3> m_wayPoints;
        [SerializeField] private Collision2DProxy m_triggerProxy;

        public Character2D Character { get; private set; }

        private Vector3 currentWayPoint;
        private int currentWayPointIndex;
        private GameObject m_target;
        
        public override void Configure(Character2D character) {
            Character = character;
            m_stateMachine.OnConfigure();
            m_stateMachine.ChangeState<PatrolState>(Character, this);
            m_triggerProxy.OnTrigger2DEnterCallback.AddListener(OnTrigger2DEnterCallback);
            m_triggerProxy.OnTrigger2DExitCallback.AddListener(OnTrigger2DExitCallback);
        }

        
        public override void Update() {
            m_stateMachine.OnUpdate(Character, this);

            m_attackSpeed -= Time.deltaTime;
        }
        
        public override void SetTarget(GameObject target) {
            m_target = target;
        }

        public override Vector3 GetDestinationPosition() {
            return m_target ? m_target.transform.position : currentWayPoint;
        }

        public override bool SearchDestination() {
            throw new System.NotImplementedException();
        }

        //o role do bot vai ser tudo aqui nessa função
        public override void MoveToDestination(Vector3 destination) {

            if (Vector2.Distance(Character.transform.position, destination) < 1f && m_target && m_attackSpeed <= 0) {
                Debug.LogError("ataquei");
                SetAction(InputAction.Button4);
                m_attackSpeed = 2f;
            }
            
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
            if (Vector3.Distance(Character.transform.position, target) < 1f) {
                return true;
            }
            return false;
        }

        public override bool IsTargetSet() {
            return m_target;
        }

        public override void SetNextDestination() {
            currentWayPoint = m_wayPoints[++currentWayPointIndex % m_wayPoints.Count];
        }

       
        private void OnTrigger2DEnterCallback(Collider2D ev) {
            if (((1 << ev.gameObject.layer) & m_targetLayer) == 0) return;
            
            SetTarget(ev.gameObject);
            m_stateMachine.ChangeState<PursuitState>(Character, this);
        }
        
        private void OnTrigger2DExitCallback(Collider2D ev) {
            if (((1 << ev.gameObject.layer) & m_targetLayer) == 0) return;
            
            SetTarget(null);
            m_stateMachine.ChangeState<PatrolState>(Character, this);
        }
    }
}