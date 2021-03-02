using UnityEngine;
using UnityEditor;

namespace RectangleTrainer.WheelOfPseudoFortune.Inspector
{
    [CustomEditor(typeof(DemoView), true)]
    public class DemoBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Roll"))
                {
                    (target as DemoView).Roll();
                }
                if (GUILayout.Button("Reload"))
                {
                    (target as DemoView).Reload();
                }
            }
        }
    }
}
