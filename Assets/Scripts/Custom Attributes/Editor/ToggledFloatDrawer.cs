using UnityEngine;
using UnityEditor;

namespace RectangleTrainer.CustomAttributes
{
    [CustomPropertyDrawer(typeof(ToggledFloatAttribute))]
    public class ToggledFloatPropertyDrawer : ToggledDrawerBase
    {
        protected override void DrawField(IToggledAttribute dynamicHide, Rect position, SerializedProperty property, GUIContent label)
        {
            ToggledFloatAttribute dynamicFloat = (ToggledFloatAttribute)dynamicHide;
            if (dynamicFloat.slider)
                EditorGUI.Slider(position, property, dynamicFloat.min, dynamicFloat.max, label);
            else
                EditorGUI.PropertyField(position, property, label, true);
        }
    }
}