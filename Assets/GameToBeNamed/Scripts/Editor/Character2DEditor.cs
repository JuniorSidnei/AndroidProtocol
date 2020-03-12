using System;
using GameToBeNamed.Character;
using Lunari.Tsuki.Editor.Extenders;
using UnityEditor;
using UnityEngine;
using Types = Lunari.Tsuki.Runtime.Types;

namespace GameToBeNamed.Editor {

    [CustomEditor(typeof(Character2D))]
    public class Character2DEditor : UnityEditor.Editor {

        private SerializedObject m_sourceRef;
        private SerializedProperty m_actions, m_input;
        private TypeSelectorButton m_actionsButton, m_inputButton;

        private void OnEnable() {
            m_sourceRef = serializedObject;
            m_actions = m_sourceRef.FindProperty("m_actions");
            m_input = m_sourceRef.FindProperty("m_input");
            m_actionsButton =
                TypeSelectorButton.Of<CharacterAction>(new GUIContent("Add action"), OnActionTypeSelected);
            m_inputButton = TypeSelectorButton.Of<InputSource>(new GUIContent("Input source"), OnInputTypeSelected);
        }

        private void OnInputTypeSelected(Type type) {
            m_input.managedReferenceValue = Activator.CreateInstance(type);
            m_input.serializedObject.ApplyModifiedProperties();
        }

        private void OnActionTypeSelected(Type type) {
            AddElementToSerializedList(m_actions, Activator.CreateInstance(type));
            m_actions.serializedObject.ApplyModifiedProperties();
        }

        private static void AddElementToSerializedList(SerializedProperty list, object value) {
            list.arraySize++;
            var element = list.GetArrayElementAtIndex(list.arraySize - 1);
            element.managedReferenceValue = value;
        }

        public override void OnInspectorGUI() {

            DisplayProperties();
            m_sourceRef.ApplyModifiedProperties();
        }

        private static void SwitchElements(SerializedProperty list, int src, int dst) {
//        SerializedProperty aux = list.GetArrayElementAtIndex(src);
            list.MoveArrayElement(src, dst);
        }

        private void DisplayProperties() {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_input.type);
            m_inputButton.OnInspectorGUI();
            EditorGUILayout.EndHorizontal();

            m_actionsButton.OnInspectorGUI();
            EditorGUI.indentLevel++;
            for (int i = 0; i < m_actions.arraySize; i++) {

                var element = m_actions.GetArrayElementAtIndex(i);
                float width = EditorGUIUtility.currentViewWidth;
                float height = EditorGUI.GetPropertyHeight(element);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Remove"))) {
                    m_actions.DeleteArrayElementAtIndex(i);
                    break;
                }

                if (i > 0) {
                    if (GUILayout.Button(new GUIContent("Up"))) {
                        m_actions.MoveArrayElement(i, i - 1);
                    }
                }

                if (i < m_actions.arraySize - 1) {
                    if (GUILayout.Button(new GUIContent("Down"))) {
                        m_actions.MoveArrayElement(i, i + 1);
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(element, new GUIContent(element.type), true);
            }

            EditorGUI.indentLevel--;
        }
    }
}