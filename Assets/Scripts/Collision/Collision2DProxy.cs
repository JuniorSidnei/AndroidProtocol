using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollisionSystem
{
    public class Collision2DProxy : MonoBehaviour
    {
        //Collisions
        private UnityEvent<Collision2D> onCollision2DEnterCallback;
        private UnityEvent<Collision2D> onCollision2DStayCallback;
        private UnityEvent<Collision2D> onCollision2DExitCallback;
        
        //Triggers
        private UnityEvent<Collider2D> onTrigger2DEnterCallback;
        private UnityEvent<Collider2D> onTrigger2DStayCallback;
        private UnityEvent<Collider2D> onTrigger2DExitCallback;

        //Collisions
        private void OnCollisionEnter2D(Collision2D other) {
            onCollision2DEnterCallback?.Invoke(other);
        }

        private void OnCollisionStay2D(Collision2D other) {
            onCollision2DStayCallback?.Invoke(other);
        }

        private void OnCollisionExit2D(Collision2D other) {
            onCollision2DExitCallback?.Invoke(other);
        }
        
        //Triggers
        private void OnTriggerEnter2D(Collider2D other) {
            onTrigger2DEnterCallback?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other) {
            onTrigger2DStayCallback?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            onTrigger2DExitCallback?.Invoke(other);
        }
        
    }
}