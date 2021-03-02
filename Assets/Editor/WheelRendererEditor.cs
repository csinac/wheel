using UnityEditor;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune.Inspector
{
    [CustomEditor(typeof(WheelRenderer))]
    public class WheelRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}