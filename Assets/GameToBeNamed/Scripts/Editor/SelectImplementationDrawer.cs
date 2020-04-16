#if UNITY_EDITOR
using System;
using System.Linq;
using GameToBeNamed.Character;
using Lunari.Tsuki.Editor.Extenders;
using Lunari.Tsuki.Runtime;
using UnityEditor;
using UnityEngine;
using Types = Lunari.Tsuki.Runtime.Types;

namespace GameToBeNamed.Editor {

    [CustomPropertyDrawer(typeof(SelectImplementation))]
    public class SelectImplementationDrawer : PropertyDrawer {
        private TypeSelectorPopupContent m_selector;
        private SerializedProperty m_property;

        private float m_height;
        private Rect m_lastRect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.Box(position, "");
            m_property = property;
            if (m_selector == null) {
                m_selector = new TypeSelectorPopupContent(
                    () => m_lastRect.width,
                    OnActionTypeSelected,
                    null,
                    TypeSelectorPopupContent.DefaultNoElementsFoundMessage,
                    ((SelectImplementation)attribute).FieldType
                );
            }
            EditorGUI.PropertyField(position, property, new GUIContent(property.managedReferenceFullTypename), true);
            
            if (Event.current.type == EventType.Repaint) {
                m_lastRect = GUILayoutUtility.GetLastRect();
            }
            
            if (GUI.Button(new Rect(position.x, position.yMax - 18, position.width, 18), "Select Implementation")) {
                PopupWindow.Show(position, m_selector);          
            }
        }

        private void OnActionTypeSelected(Type obj) {
            m_property.managedReferenceValue = Activator.CreateInstance(obj);
            m_property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            m_height = EditorGUI.GetPropertyHeight(property, true) + 18;
            return m_height;
        }
    }
}
#endif