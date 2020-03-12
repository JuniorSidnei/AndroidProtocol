using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameToBeNamed.Utils {
	public class QueuedEventDispatcher {

		//helper class definitions
		private abstract class BaseEvent {
			public abstract void Invoke(BaseDataHolder dataHolder);
		}

		private sealed class Event<T> : BaseEvent {

			public Event() {
				m_listeners = new List<UnityAction<T>>();
				m_listenersToRemove = new List<UnityAction<T>>();
				m_listenersToAdd = new List<UnityAction<T>>();
			}

			public void Subscribe(UnityAction<T> callback) {
				if (m_invoking) {
					m_listenersToAdd.Add(callback);
				}
				else {
					m_listeners.Add(callback);
				}
			}

			public void Unsubscribe(UnityAction<T> callback) {
				if (m_invoking) {
					m_listenersToRemove.Add(callback);
				}
				else {
					m_listeners.Remove(callback);
				}
			}

			public override void Invoke(BaseDataHolder dataHolder) {
				var castedDataHolder = dataHolder as DataHolder<T>;

				if (castedDataHolder == null) {
					throw new InvalidCastException();
				}

				m_invoking = true;
				foreach (var listener in m_listeners) {
					listener(castedDataHolder.Data);
				}

				m_invoking = false;
				if (m_listenersToRemove.Count > 0) {
					foreach (var listener in m_listenersToRemove) {
						m_listeners.Remove(listener);
					}

					m_listenersToRemove.Clear();
				}

				if (m_listenersToAdd.Count > 0) {
					m_listeners.AddRange(m_listenersToAdd);
					m_listenersToAdd.Clear();
				}
			}

			public bool IsSubscribed(UnityAction<T> callback) {
				return m_listeners.Find(c => c == callback) != null;
			}

			public int ListenerCount {
				get { return m_listeners.Count; }
			}

			private bool m_invoking;

			private List<UnityAction<T>> m_listeners;
			private List<UnityAction<T>> m_listenersToRemove;
			private List<UnityAction<T>> m_listenersToAdd;
		}

		private class BaseDataHolder {
			public PropertyName PropertyName;
		}

		private class DataHolder<T> : BaseDataHolder {
			public DataHolder(T data) {
				PropertyName = new PropertyName(typeof(T).Name);
				Data = data;
			}

			public T Data;
		}
		//-------------------------------------------------

		private Dictionary<PropertyName, BaseEvent> m_eventMap;
		private Queue<BaseDataHolder> m_dataHolderQueue;

		public QueuedEventDispatcher() {
			m_eventMap = new Dictionary<PropertyName, BaseEvent>();
			m_dataHolderQueue = new Queue<BaseDataHolder>();
		}

		public void DispatchAll() {
			while (m_dataHolderQueue.Count > 0) {
				DispatchEvent(m_dataHolderQueue.Dequeue());
			}
		}

		public void Emit<T>(T data) {
			var type = new PropertyName(typeof(T).Name);
			if (!m_eventMap.ContainsKey(type)) {
				return;
			}

			m_dataHolderQueue.Enqueue(new DataHolder<T>(data));
		}

		public void EmitImmediate<T>(T data) {
			var type = new PropertyName(typeof(T).Name);
			if (!m_eventMap.ContainsKey(type)) {
				return;
			}

			DispatchEvent(new DataHolder<T>(data));
		}

		public bool IsSubscribed<T>(UnityAction<T> callback) {
			var type = new PropertyName(typeof(T).Name);
			if (!m_eventMap.ContainsKey(type)) {
				m_eventMap.Add(type, new Event<T>());
			}

			var ev = m_eventMap[type] as Event<T>;
			if (ev == null) {
				throw new InvalidCastException();
			}

			return ev.IsSubscribed(callback);
		}

		public void Subscribe<T>(UnityAction<T> callback) {
			var type = new PropertyName(typeof(T).Name);
			if (!m_eventMap.ContainsKey(type)) {
				m_eventMap.Add(type, new Event<T>());
			}

			var ev = m_eventMap[type] as Event<T>;
			if (ev == null) {
				throw new InvalidCastException();
			}

			ev.Subscribe(callback);
		}

		public bool HasListeners<T>() {
			var type = new PropertyName(typeof(T).Name);
			if (!m_eventMap.ContainsKey(type)) {
				return false;
			}

			var ev = m_eventMap[type] as Event<T>;
			if (ev == null) {
				throw new InvalidCastException();
			}

			return ev.ListenerCount > 0;
		}

		public void Unsubscribe<T>(UnityAction<T> callback) {
			var type = new PropertyName(typeof(T).Name);
			if (!m_eventMap.ContainsKey(type)) {
				return;
			}

			var ev = m_eventMap[type] as Event<T>;
			if (ev == null) {
				throw new InvalidCastException();
			}

			ev.Unsubscribe(callback);
		}

		private void DispatchEvent(BaseDataHolder dataHolder) {
			if (!m_eventMap.ContainsKey(dataHolder.PropertyName)) {
				return;
			}

			m_eventMap[dataHolder.PropertyName].Invoke(dataHolder);
		}

	}
}