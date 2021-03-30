using UnityEngine;
using UnityEditor;

namespace RectangleTrainer.CustomAttributes
{
    [CustomPropertyDrawer(typeof(ToggledIntAttribute))]
    public class ToggledIntPropertyDrawer : ToggledDrawerBase
    {
        protected override void DrawField(IToggledAttribute dynamicHide, Rect position, SerializedProperty property, GUIContent label)
        {
            ToggledIntAttribute dynamicFloat = (ToggledIntAttribute)dynamicHide;
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}