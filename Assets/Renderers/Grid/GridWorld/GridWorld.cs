using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class GridWorld : GridBase
    {
        [Header("World Grid Options")]
        [SerializeField] protected GridCellWorld cellPF;

        protected override UnitRenderer MakeUnit(int order, WheelBase slot)
        {
            float x, y;
            CalculateCellPosition(order, out x, out y);

            GridCellWorld cell = Instantiate(cellPF);
            cell.Initialize(x, y, unitContainer);
            cell.SetContent(slot.name);

            return cell;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            
            if(priority == SizingPriority.MatchColumns)
            {
                if (columns == 0) columns = 1;

                float span = cellStep.x * columns / 2f;
                Gizmos.DrawLine(new Vector3(-span, -span,  0.5f), new Vector3(-span, span,  0.5f));
                Gizmos.DrawLine(new Vector3(-span, -span, -0.5f), new Vector3(-span, span, -0.5f));

                Gizmos.DrawLine(new Vector3( span, -span,  0.5f), new Vector3( span, span,  0.5f));
                Gizmos.DrawLine(new Vector3( span, -span, -0.5f), new Vector3( span, span, -0.5f));

                Gizmos.DrawWireMesh(Mesh2Script.ScriptMesh.GizmoArrow_ScriptMesh.Mesh, new Vector3(0, span, 0));

                Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(transform.eulerAngles.x + 180, transform.eulerAngles.y, transform.eulerAngles.z), transform.lossyScale);
                Gizmos.DrawWireMesh(Mesh2Script.ScriptMesh.GizmoArrow_ScriptMesh.Mesh, new Vector3(0, span, 0));
            }
            else
            {
                if (rows == 0) rows = 1;

                float span = cellStep.y * rows / 2f;
                Gizmos.DrawLine(new Vector3(-span, span,  0.5f), new Vector3( span, span,  0.5f));
                Gizmos.DrawLine(new Vector3(-span, span, -0.5f), new Vector3( span, span, -0.5f));

                Gizmos.DrawLine(new Vector3(-span, -span,  0.5f), new Vector3(span, -span, 0.5f));
                Gizmos.DrawLine(new Vector3(-span, -span, -0.5f), new Vector3(span, -span, -0.5f));

                Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, 0, 0), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90), transform.lossyScale);
                Gizmos.DrawWireMesh(Mesh2Script.ScriptMesh.GizmoArrow_ScriptMesh.Mesh, new Vector3(0, span, 0));

                Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3( 0, 0, 0), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90), transform.lossyScale);
                Gizmos.DrawWireMesh(Mesh2Script.ScriptMesh.GizmoArrow_ScriptMesh.Mesh, new Vector3(0, span, 0));
            }
        }
    }
}
