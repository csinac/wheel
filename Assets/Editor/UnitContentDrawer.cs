using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune.Inspector
{
    [CustomPropertyDrawer(typeof(UnitContent<>))]
    public class UnitContentDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float width = position.width;
            Rect keyRect = new Rect(position.x, position.y, width / 2 - 5, position.height);
            Rect imgRect = new Rect(position.x + width / 2, position.y, width / 2, position.height);

            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(imgRect, property.FindPropertyRelative("content"), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
