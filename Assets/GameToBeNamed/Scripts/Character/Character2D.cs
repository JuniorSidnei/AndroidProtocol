using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Character {
	
	public class Character2D : MonoBehaviour
	{

		[HideInInspector]
		public Vector2 Velocity;
		private float m_drag;
		
		private Controller2D m_controller2D;

		[SerializeField] private AnimatorProxy m_animatorProxy;

		public AnimatorProxy AnimatorProxy {
			get{ return m_animatorProxy; }	
		}

		public Controller2D Controller2D {
			get { return m_controller2D; }
			set { m_controller2D = value; }
		}

		public float Drag {
			get { return m_drag; }
			set { m_drag = value; }
		}
		
		[Flags]
		public enum Status {
			None 		= 0,
			OnGround 	= 1 << 1,
			OnWall 		= 1 << 2,
			Moving 		= 1 << 3,
			Falling 	= 1 << 4,
			Attack 	    = 1 << 5,
			Dead 		= 1 << 6,
			Jumping		= 1 << 7,
			Blocking	= 1 << 8
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
			
			foreach (var action in m_actions) {
				action.Configure(this);
			}
			m_input.Configure();
			m_controller2D = GetComponent<Controller2D>();
		}

		
		private void Update() {
			m_input.Update();
			LocalDispatcher.Emit(new OnCharacterUpdate());
			LocalDispatcher.DispatchAll();
			
			
			m_controller2D.Move(Velocity * Time.deltaTime);
			Velocity *= (1 - Time.deltaTime * m_drag);
		}
	}
}