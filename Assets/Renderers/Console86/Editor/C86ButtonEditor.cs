using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [CustomEditor(typeof(C86Button))]
    public class C86ButtonEditor : Editor
    {
        /// <summary>
        /// Exposes custom color fields in the derived button class
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
