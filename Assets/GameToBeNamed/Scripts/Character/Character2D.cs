using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character {
	
	public class Character2D : MonoBehaviour {

		[SerializeReference, SelectImplementation(typeof(ICharacterAction))]
		private List<ICharacterAction> m_actions = new List<ICharacterAction>();
		
		[SerializeReference, SelectImplementation(typeof(IInputSource))]
		private IInputSource m_input = new PlayerInput();
		
		public readonly QueuedEventDispatcher LocalDispatcher = new QueuedEventDispatcher();
		
		public readonly Dictionary<PropertyName, bool> ActionStatus = new Dictionary<PropertyName, bool>();
		
		public IInputSource Input => m_input;

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