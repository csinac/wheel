using UnityEngine;
using UnityEditor;

namespace RectangleTrainer.CustomAttributes
{
    [CustomPropertyDrawer(typeof(ToggledObjectAttribute))]
    public class ToggledObjectPropertyDrawer : ToggledDrawerBase
    {
        protected override void DrawField(IToggledAttribute dynamicHide, Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}