using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

		[SerializeField] private BaseAnimatorProxy m_animatorProxy;
		
		public Controller2D Controller2D {
			get { return m_controller2D; }
			set { m_controller2D = value; }
		}

		public float Drag {
			get { return m_drag; }
			set { m_drag = value; }
		}
		
		public readonly Dictionary<PropertyName, bool> ActionStatus = new Dictionary<PropertyName, bool>();
		
		[SerializeReference, SelectImplementation(typeof(ICharacterAction))]
		private List<ICharacterAction> m_actions = new List<ICharacterAction>();
		
		[SerializeReference, SelectImplementation(typeof(IInputSource))]
		private IInputSource m_inputSource = new PlayerInput();
		
		public readonly QueuedEventDispatcher LocalDispatcher = new QueuedEventDispatcher();
		
		public IInputSource Input => m_inputSource;
		
		private void Awake() {		
			
			foreach (var action in m_actions) {
				action.Configure(this);
			}
			
			m_controller2D = GetComponent<Controller2D>();
			m_inputSource.Configure();
		}

		
		private void Update() {
			m_inputSource.Update();
			LocalDispatcher.Emit(new OnCharacterUpdate());
			LocalDispatcher.DispatchAll();
		}

		private void FixedUpdate() {
			LocalDispatcher.EmitImmediate(new OnCharacterFixedUpdate());
			
			m_controller2D.Move(Velocity * Time.deltaTime);
			Velocity *= (1 - Time.deltaTime * m_drag);
		}

		public void AddAction(ICharacterAction action) {
			//veririfcar se existe
			action.Configure(this);
			m_actions.Add(action);
		}
	}
}