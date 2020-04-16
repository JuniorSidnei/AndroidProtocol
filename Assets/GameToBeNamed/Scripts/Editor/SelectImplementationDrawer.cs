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
        private bool m_expanded;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
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

            if (property.isExpanded) {
                GUI.Box(position, "");
            }
            
            EditorGUI.PropertyField(position, property, new GUIContent(FilterTypeName(property.managedReferenceFullTypename)), true); 
            if (property.isExpanded) {
                if (Event.current.type == EventType.Repaint) {
                    m_lastRect = GUILayoutUtility.GetLastRect();
                }
            
                if (GUI.Button(new Rect(position.x, position.yMax - 18, position.width, 18), "Select Implementation")) {
                    PopupWindow.Show(position, m_selector);          
                }    
            }
        }

        private void OnActionTypeSelected(Type obj) {
            m_property.managedReferenceValue = Activator.CreateInstance(obj);
            m_property.serializedObject.ApplyModifiedProperties();
        }

        private string FilterTypeName(string fullName) {
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            m_height = EditorGUI.GetPropertyHeight(property, true) + (property.isExpanded ? 18 : 0);
            return m_height;
        }
    }
}
#endif