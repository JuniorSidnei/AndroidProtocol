using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
	
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

			foreach (var action in m_actions) {
				action.Configure(this);
			}
		}

		private void Update() {
			m_input.Update();
			LocalDispatcher.Emit(new OnCharacterUpdate());
			LocalDispatcher.DispatchAll();
		}
	}
}