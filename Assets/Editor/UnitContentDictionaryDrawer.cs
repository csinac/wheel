using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune.Inspector
{
    [CustomPropertyDrawer(typeof(UnitContentDictionary<>))]
    public class UnitContentDictionaryDrawer : PropertyDrawer
    {
        private SerializedProperty list;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string ogLabel = label.text;
            position.height = EditorGUI.GetPropertyHeight(GetList(property));
            label.text = ogLabel;

            EditorGUI.BeginProperty(position, label, property);            
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.PropertyField(position, GetList(property), GUIContent.none, true);

            if(GetList(property).isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect fallbackRect = position;
                fallbackRect.height = EditorGUIUtility.singleLineHeight;
                fallbackRect.y = EditorGUI.GetPropertyHeight(list) + EditorGUIUtility.singleLineHeight * 2;
                EditorGUI.ObjectField(fallbackRect, property.FindPropertyRelative("fallback"), new GUIContent("Default Content"));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty list = property.FindPropertyRelative("inspectorList");
            if(list.isExpanded)
                return EditorGUI.GetPropertyHeight(list) + EditorGUIUtility.singleLineHeight * 2;

            return EditorGUI.GetPropertyHeight(list);
        }

        private SerializedProperty GetList(SerializedProperty property)
        {
            if (list == null)
                list = property.FindPropertyRelative("inspectorList");

            return list;
        }

    }
}
