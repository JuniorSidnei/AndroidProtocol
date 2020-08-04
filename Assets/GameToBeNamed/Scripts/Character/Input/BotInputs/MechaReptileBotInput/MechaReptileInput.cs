using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {

    [System.Serializable]
    public class MechaReptileInput : BotInput {
        
        [SerializeField] private LayerMask m_targetLayer;
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
            m_triggerProxy.OnCollision2DExitCallback.AddListener(OnCollision2DExitCallback);
        }
        

        public override void Update() {
            m_stateMachine.OnUpdate(Character, this);
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
            
            Debug.Log("meu target está aqui");
            SetTarget(ev.gameObject);
            m_stateMachine.ChangeState<PursuitState>(Character, this);
        }

        private void OnCollision2DExitCallback(Collision2D ev) {
            if (((1 << ev.gameObject.layer) & m_targetLayer) == 0) return;
            
            Debug.Log("meu target não está aqui");
            m_stateMachine.RevertToPreviousState(Character, this);
        }
    }
}