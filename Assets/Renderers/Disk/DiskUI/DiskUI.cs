using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class DiskUI : DiskBase<Sprite>
    {
        [Header("UI Disk Options")]
        [SerializeField] private DiskSliceUIBase slicePF;

        public void Reset()
        {
            radius = 200;
            pegSize = 50;
        }

        protected override UnitRenderer MakeUnit(int order, WheelBase slot)
        {
            if (slicePF == null)
                throw new Exceptions.MissingUnitPFException("slicePF", typeof(DiskSliceUIBase));

            DiskSliceUIBase slice = Instantiate(slicePF);
            slice.Initialize(radius: radius,
                             arc: arc,
                             parent: unitContainer,
                             index: order);

            SetContent(ref slice, slot.name);
            slice.gameObject.name = $"UI Slice_{slot.name}";
            return slice;
        }

        override protected void OnDrawGizmos()
        {
            if (!Application.isPlaying && !PreviewRenderer && unitContainer)
            {
                Gizmos.color = Color.white;
                Gizmos.matrix = unitContainer.transform.localToWorldMatrix;

                int res = 18;
                Vector3 prev = MakeVertex(0);
                for (int i = 0; i <= res; i++)
                {
                    Vector3 vert = MakeVertex(2 * Mathf.PI * i / res);
                    Gizmos.DrawLine(prev, vert);
                    prev = vert;
                }
            }

            Vector3 MakeVertex(float angle)
            {
                return new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            }
        }
    }
}