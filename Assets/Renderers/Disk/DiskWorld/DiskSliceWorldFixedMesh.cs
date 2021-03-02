using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class DiskSliceWorldFixedMesh : DiskSliceWorld
    {
        [SerializeField] Mesh mesh;
        protected override Mesh Mesh()
        {
            return mesh;
        }
    }
}
