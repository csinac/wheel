using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class DiskWorld : DiskBase<GameObject>
    {
        [Header("World Disk Options")]
        [SerializeField] protected float thickness = 0.1f;
        [SerializeField] protected DiskSliceWorld slicePF;

        public void Reset()
        {
            radius = 1;
            pegSize = 1;

            fontSize = 60;
            contentOffset = 0.5f;
            contentScale = 0.5f;
        }

        protected override UnitRenderer MakeUnit(int order, WheelBase slot)
        {
            if (slicePF == null)
                throw new Exceptions.MissingUnitPFException("slicePF", typeof(DiskSliceWorld));

            DiskSliceWorld slice = Instantiate(slicePF);
            slice.Initialize(arc: arc,
                             radius: radius,
                             thickness: thickness,
                             parent: unitContainer,
                             index: order);

            SetContent(ref slice, slot.name);
            slice.gameObject.name = $"World Slice_{slot.name}";
            return slice;
        }

        override protected void OnDrawGizmos()
        {
            if(!Application.isPlaying && !hideFlags.HasFlag(HideFlags.HideInHierarchy))
            {
                Vector3 scale = new Vector3(transform.lossyScale.x * radius, transform.lossyScale.y * radius, transform.lossyScale.z * thickness);
                Gizmos.color = Color.white;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
                Gizmos.DrawWireMesh(Placeholders.WorldWheel.Mesh);
            }
        }
    }
}
