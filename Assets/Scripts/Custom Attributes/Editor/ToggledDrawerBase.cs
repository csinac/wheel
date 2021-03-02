using UnityEngine;
using UnityEditor;

namespace RectangleTrainer.CustomAttributes
{
    public abstract class ToggledDrawerBase : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IToggledAttribute dynamicHide = (IToggledAttribute)attribute;
            bool enabled = GetEnabled(dynamicHide, property);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (enabled)
            {
                DrawField(dynamicHide, position, property, label);
            }

            GUI.enabled = wasEnabled;
        }

        abstract protected void DrawField(IToggledAttribute dynamicHide, Rect position, SerializedProperty property, GUIContent label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            IToggledAttribute dynamicHide = (IToggledAttribute)attribute;
            bool enabled = GetEnabled(dynamicHide, property);

            if (enabled)
                return EditorGUI.GetPropertyHeight(property, label);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }

        private bool GetEnabled(IToggledAttribute dynamicHide, SerializedProperty property)
        {
            bool enabled = true;
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, dynamicHide.Controller);
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                if (dynamicHide.Type == ControlType.Integer)
                    enabled = sourcePropertyValue.intValue == dynamicHide.Condition;
                else if (dynamicHide.Type == ControlType.Boolean)
                    enabled = sourcePropertyValue.boolValue;
            }

            return enabled;
        }
    }
}