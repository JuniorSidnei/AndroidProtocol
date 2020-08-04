using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Utils{
    
    [RequireComponent(typeof(Collider2D))]
    public class Collision2DProxy : MonoBehaviour {

        public float LineOfSite;
        public class TriggerEvent : UnityEvent<Collider2D> { }
        public class CollisionEvent : UnityEvent<Collision2D> { }
        
        //Collisions
        public CollisionEvent OnCollision2DEnterCallback = new CollisionEvent();
        public CollisionEvent OnCollision2DStayCallback = new CollisionEvent();
        public CollisionEvent OnCollision2DExitCallback = new CollisionEvent();

        //Triggers
        public TriggerEvent OnTrigger2DEnterCallback = new TriggerEvent();
        public TriggerEvent OnTrigger2DStayCallback = new TriggerEvent();
        public TriggerEvent OnTrigger2DExitCallback = new TriggerEvent();

        public Collider2D BoxCollider => m_boxCollider2D != null ? m_boxCollider2D : (m_boxCollider2D = GetComponent<BoxCollider2D>());
        public Collider2D CircleCollider => m_circleCollider2D != null ? m_circleCollider2D : (m_circleCollider2D = GetComponent<CircleCollider2D>());

        private BoxCollider2D m_boxCollider2D;
        private CircleCollider2D m_circleCollider2D;

        //Collisions
        private void OnCollisionEnter2D(Collision2D other) {
            OnCollision2DEnterCallback?.Invoke(other);
        }

        private void OnCollisionStay2D(Collision2D other) {
            OnCollision2DStayCallback?.Invoke(other);
        }

        private void OnCollisionExit2D(Collision2D other) {
            OnCollision2DExitCallback?.Invoke(other);
        }

        //Triggers
        private void OnTriggerEnter2D(Collider2D other) {
            OnTrigger2DEnterCallback?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other) {
            OnTrigger2DStayCallback?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            OnTrigger2DExitCallback?.Invoke(other);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, LineOfSite);
        }
    }
}