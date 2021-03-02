using UnityEditor;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Inspector
{
    public class DiskBaseEditor<T> : Editor where T : Object
    {
        DiskBase<T> renderer;

        private void Awake()
        {
            renderer = target as DiskBase<T>;
            Undo.undoRedoPerformed += () =>
            {
                DestroyPreviews();
                renderer.SetPegTransform();
                renderer.SetPegSize();
            };
        }

        public override void OnInspectorGUI()
        {
            bool previewCreated = PreviewSection();

            float pegOffset = renderer.PegOffset;
            float pegAngle = renderer.PegAngle;
            float pegSize = renderer.PegSize;
            float radius = renderer.Radius;

            base.OnInspectorGUI();

            if (pegAngle != renderer.PegAngle ||
               pegOffset != renderer.PegOffset ||
               radius != renderer.Radius)
            {
                renderer.SetPegTransform();
            }

            if (pegSize != renderer.PegSize)
                renderer.SetPegSize();

            if (GUI.changed && !previewCreated)
                DestroyPreviews();
        }


        #region Preview

        GameObject clone;
        int prevCount = 8;
        string prevLabel = "";
        T prevContent;
        bool folderOpen = false;
        bool disposablePreview = true;

        private bool PreviewSection()
        {
            if (!Application.isPlaying)
            {
                if (GUILayout.Button($"{ (folderOpen ? "Hide" : "Show") } Preview Panel")) folderOpen = !folderOpen;
                if (folderOpen)
                {
                    disposablePreview = EditorGUILayout.Toggle("Disposable Previews", disposablePreview);

                    EditorGUILayout.PrefixLabel("Unit Count");
                    prevCount = EditorGUILayout.IntSlider(prevCount, 2, 29);

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PrefixLabel("Preview Label");
                    prevLabel = EditorGUILayout.TextField(prevLabel);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PrefixLabel("Preview Content");
                    prevContent = (T)EditorGUILayout.ObjectField(prevContent, typeof(T), false);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button("Generate Preview"))
                    {
                        GeneratePreview(prevCount, prevLabel, prevContent);
                        return true;
                    }
                }
                EditorGUILayout.Space();
            }
            return false;
        }

        private void GeneratePreview(int count, string previewLabel = "", T previewContent = default)
        {
            DestroyPreviews();

            clone = Instantiate(renderer.gameObject, renderer.transform.parent);
            clone.name = $"{renderer.gameObject.name} Preview";

            if (disposablePreview)
                clone.hideFlags = HideFlags.HideInHierarchy;

            DiskBase<T> cloneRenderer = clone.GetComponent<DiskBase<T>>();
            cloneRenderer.SetContentDictionary(new UnitContentDictionary<T>(previewContent));

            Model.Wheel wheel = Model.Wheel.Create();
            for (int i = 0; i < count; i++)
            {
                string label = previewLabel == "" ? Lipsum.Next : previewLabel;
                wheel.AddSlot(Model.Wheel.Create(name: label));
            }

            cloneRenderer.SetWheel(wheel);
        }

        /// <summary>
        /// Deletes all hidden renderers
        /// </summary>
        private void DestroyPreviews()
        {
            WheelRenderer[] views = FindObjectsOfType<WheelRenderer>();
            for (int i = 0; i < views.Length; i++)
            {
                if (views[i].PreviewRenderer)
                    DestroyImmediate(views[i].gameObject);
            }

            return;
        }

        private void OnDestroy()
        {
            DestroyPreviews();
        }

        #endregion
    }

    [CustomEditor(typeof(DiskUI), true), CanEditMultipleObjects]
    public class DiskUIEditor : DiskBaseEditor<Sprite> { }

    [CustomEditor(typeof(DiskWorld), true), CanEditMultipleObjects]
    public class DiskWorldEditor : DiskBaseEditor<GameObject> { }
}