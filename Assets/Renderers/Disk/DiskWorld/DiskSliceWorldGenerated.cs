using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [RequireComponent(typeof(MeshFilter))]
    public class DiskSliceWorldGenerated : DiskSliceWorld
    {
        [SerializeField] protected int arcResolution = 8;
        [SerializeField, Range(0.5f, 1f)] protected float arcFill = 0.975f;
        [SerializeField] protected float zOffset = 0.01f;

        protected override Mesh Mesh()
        {
            return WorldSliceMaker.Generate(arc * arcFill, radius, thickness, arcResolution);
        }

        public override void Initialize(float arc, float radius, float thickness, Transform parent, int index)
        {
            base.Initialize(arc, radius, thickness, parent, index);

            if (contentContainer)
                contentContainer.localPosition = new Vector3(0, 0, -(thickness + zOffset));
            else
                Debug.LogWarning("Content Container is missing. Skipping thickness correction.");
        }
    }
}
