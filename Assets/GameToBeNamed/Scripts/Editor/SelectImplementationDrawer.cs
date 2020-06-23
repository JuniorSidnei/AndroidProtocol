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

        private bool isReferenceValid(SerializedProperty property) {
            return !property.managedReferenceFullTypename.IsNullOrEmpty();
        }

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

            if (isReferenceValid(property)) {
                DrawProperty(position, property);
            }
            else {
                //property.isExpanded = EditorGUI.Foldout (position, property.isExpanded, new GUIContent("Empty"));

                //if (property.isExpanded) {
                    DrawImplementationButton(position, "Select Implementation");
                //}
            }
        }

        private void DrawProperty(Rect position, SerializedProperty property) {

            if (property.isExpanded) {
                GUI.Box(position, "");
            }

            EditorGUI.PropertyField(position, property,
                new GUIContent(FilterTypeName(property.managedReferenceFullTypename)), true);

            if (property.isExpanded) {
                DrawImplementationButton(position, "Change Implementation");
            }
        }

        private void DrawImplementationButton(Rect position, string name) {
            
            if (Event.current.type == EventType.Repaint) {
                m_lastRect = GUILayoutUtility.GetLastRect();
            }
            
            if (GUI.Button(new Rect(position.x, position.yMax - 18, position.width, 18), name)) {
                PopupWindow.Show(m_lastRect, m_selector);          
            }    
        }
        
        private void OnActionTypeSelected(Type obj) {
            m_property.serializedObject.Update();
            m_property.managedReferenceValue = Activator.CreateInstance(obj);
            m_property.serializedObject.ApplyModifiedProperties();
        }

        private string FilterTypeName(string fullName) {
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

           // if (isReferenceValid(property)) {
                m_height = EditorGUI.GetPropertyHeight(property, true) + (property.isExpanded ? 24 : 0);
            //}
//            else {
//                
//                m_height = EditorGUI.GetPropertyHeight(property, true);
//                
//                if (property.isExpanded) {
//                    m_height += 18;
//                }
//            }
            return m_height;
        }
    }
}
#endif