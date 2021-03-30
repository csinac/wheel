/*
 * This script is created by using mesh2script
 * available at https://github.com/rectdev/mesh2script
 * Rectangle Trainer, 2021
 */

using UnityEngine;
namespace RectangleTrainer.Mesh2Script.ScriptMesh
{
	public static class GizmoArrow_ScriptMesh
	{
		private static Mesh mesh;

        private static float[] vertList = 
		{
            0.25f, 0.144418f, 0.25f,0f, 0.113459f, 0.108056f,0.108056f, 0.113459f, 0f,-0.108056f, 0.113459f, 0f,
			0f, 0.113459f, 0.108056f,-0.25f, 0.144418f, 0.25f,0f, 0.113459f, -0.108056f,
			-0.108056f, 0.113459f, 0f,-0.25f, 0.144418f, -0.25f,0.25f, 0.144418f, -0.25f,
			0.108056f, 0.113459f, 0f,0f, 0.113459f, -0.108056f,0f, 0.644418f, 0f,
			-0.25f, 0.144418f, 0.25f,0.25f, 0.144418f, 0.25f,-0.25f, 0.144418f, -0.25f,
			-0.25f, 0.144418f, 0.25f,0f, 0.644418f, 0f,-0.25f, 0.144418f, -0.25f,
			0f, 0.644418f, 0f,0.25f, 0.144418f, -0.25f,0f, 0.644418f, 0f,
			0.25f, 0.144418f, 0.25f,0.25f, 0.144418f, -0.25f,0f, 0.113459f, 0.108056f,
			0f, -0.202356f, 0f,0.108056f, 0.113459f, 0f,0f, 0.113459f, -0.108056f,
			0.108056f, 0.113459f, 0f,0f, -0.202356f, 0f,0f, 0.113459f, -0.108056f,
			0f, -0.202356f, 0f,-0.108056f, 0.113459f, 0f,0f, 0.113459f, 0.108056f,
			-0.108056f, 0.113459f, 0f,0f, -0.202356f, 0f
		};

		private static int[] triangles =
		{
            0, 1, 2,3, 4, 5,6, 7, 8,9, 10, 11,12, 13, 14,15, 16, 17,18, 19, 20,
			21, 22, 23,24, 25, 26,27, 28, 29,30, 31, 32,33, 34, 35,
		};

        private static float[] nList = 
		{
            
		};

        private static float[] uvList = 
		{
            
		};

		public static Mesh Mesh
		{
			get
			{
				if (mesh == null)
					mesh = Make();

				return mesh;
			}
		}

        private static Vector3[] FloatListToVector3(ref float[] list) {
			Vector3[] vectorArray = new Vector3[list.Length / 3];
            for(int i = 0; i < vectorArray.Length; i ++) {
                vectorArray[i] = new Vector3(list[i * 3], list[i * 3 + 1], list[i * 3 + 2]);
            }

            return vectorArray;
        }

        private static Vector2[] FloatListToVector2(ref float[] list) {
			Vector2[] vectorArray = new Vector2[list.Length / 2];
            for(int i = 0; i < vectorArray.Length; i ++) {
                vectorArray[i] = new Vector2(list[i * 2], list[i * 2 + 1]);
            }

            return vectorArray;
        }

		private static Mesh Make()
		{
			Vector3[] vertices = FloatListToVector3(ref vertList);
			Vector3[] normals = FloatListToVector3(ref nList);
            Vector2[] uv = FloatListToVector2(ref uvList);

			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
            mesh.uv = uv;

            if(mesh.normals.Length == 0) {
                mesh.RecalculateNormals();
            }

            mesh.RecalculateBounds();

			return mesh;
		}
	}
}
