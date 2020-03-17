using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Utils{
    
    [RequireComponent(typeof(Collider2D))]
    public class Collision2DProxy : MonoBehaviour {
        
        //Collisions
        public UnityEvent<Collision2D> OnCollision2DEnterCallback;
        public UnityEvent<Collision2D> OnCollision2DStayCallback;
        public UnityEvent<Collision2D> OnCollision2DExitCallback;

        //Triggers
        public UnityEvent<Collider2D> OnTrigger2DEnterCallback;
        public UnityEvent<Collider2D> OnTrigger2DStayCallback;
        public UnityEvent<Collider2D> OnTrigger2DExitCallback;
        

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
    }
}