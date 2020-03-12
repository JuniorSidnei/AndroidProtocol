using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSystem {
	
	public class Character2D : MonoBehaviour {

		[Flags]
		public enum Status {
			None 		= 0,
			OnGround 	= 1 << 1,
			OnWall 		= 1 << 2,
			Moving 		= 1 << 3,
			Falling 	= 1 << 4,
			Burning 	= 1 << 5,
			Dead 		= 1 << 6
		}

		[SerializeReference] 
		private List<ICharacterAction> m_actions = new List<ICharacterAction>();
		
		[SerializeReference]
		private IInputSource m_input;
		
		private Status m_status;
		
		public readonly QueuedEventDispatcher LocalDispatcher = new QueuedEventDispatcher();
		
		public IInputSource Input => m_input;

		public void SetStatus(Status status) {
			m_status |= status;
		}

		public void UnsetStatus(Status status) {
			m_status &= ~status;
		}

		public bool HasStatus(Status status) {
			return (m_status & status) != 0;
		}

		private void Awake() {

//			Bounds bounds = GetComponent<Collider2D>().bounds;
//			m_bottomOffset = new Vector2(0, -bounds.extents.y);
//			m_rightOffset = new Vector2(bounds.extents.x, 0);
//			m_leftOffset = new Vector2(-bounds.extents.x, 0);
//			m_collisionRadius = bounds.extents.x;

			

			foreach (var action in m_actions) {
				action.Configure(this);
			}
		}

		private void Update() {
//			CheckCollisions();
			m_input.Update();
			LocalDispatcher.Emit(new OnCharacterUpdate());
			LocalDispatcher.DispatchAll();
		}

		private void OnCollisionEnter(Collision other) {
			LocalDispatcher.Emit(new OnCharacterCollisionEnter(other.gameObject));
		}

//		public void CheckCollisions() {
//			if (Physics2D.OverlapCircle((Vector2) transform.position + m_bottomOffset, m_collisionRadius, CollisionMask)) {
//				SetStatus(Status.OnGround);
//			}
//			else {
//				UnsetStatus(Status.OnGround);
//			}
//
//			if (Physics2D.OverlapCircle((Vector2) transform.position + m_rightOffset, m_collisionRadius, CollisionMask)
//			    || Physics2D.OverlapCircle((Vector2) transform.position + m_leftOffset, m_collisionRadius,
//				    CollisionMask)) {
//				SetStatus(Status.OnWall);
//			}
//			else {
//				UnsetStatus(Status.OnWall);
//			}
//		}

//		private void OnDrawGizmos() {
//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere((Vector2) transform.position + m_bottomOffset, m_collisionRadius);
//            Gizmos.DrawWireSphere((Vector2) transform.position + m_rightOffset, m_collisionRadius);
//            Gizmos.DrawWireSphere((Vector2) transform.position + m_leftOffset, m_collisionRadius);
//        }
		
	}
}