using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer.Placeholders
{
    /// <summary>
    /// Recreates a hollow, 18 side cylinder of height and radius of one,
    /// aligned to z-axis.
    /// </summary>
    public static class WorldWheel
    {
        private static Mesh mesh;
        public static Mesh Mesh
        {
            get
            {
                if (mesh == null)
                    mesh = Make();

                return mesh;
            }
        }

        private static Mesh Make()
        {
            Vector3[] vertices = {
                new Vector3(0.0000f, 0.000000f, 0.000000f),
                new Vector3(0.000000f, 1.000000f, -1.000000f),
                new Vector3(-0.342020f, 0.939693f, -1.000000f),
                new Vector3(-0.642788f, 0.766045f, -1.000000f),
                new Vector3(-0.866025f, 0.500000f, -1.000000f),
                new Vector3(-0.984808f, 0.173648f, -1.000000f),
                new Vector3(-0.984808f, -0.173648f, -1.000000f),
                new Vector3(-0.866025f, -0.500000f, -1.000000f),
                new Vector3(-0.642788f, -0.766044f, -1.000000f),
                new Vector3(-0.342020f, -0.939692f, -1.000000f),
                new Vector3(-0.000000f, -1.000000f, -1.000000f),
                new Vector3(0.342020f, -0.939692f, -1.000000f),
                new Vector3(0.642787f, -0.766044f, -1.000000f),
                new Vector3(0.866025f, -0.500000f, -1.000000f),
                new Vector3(0.984808f, -0.173648f, -1.000000f),
                new Vector3(0.984808f, 0.173648f, -1.000000f),
                new Vector3(0.866026f, 0.500000f, -1.000000f),
                new Vector3(0.642788f, 0.766044f, -1.000000f),
                new Vector3(0.342021f, 0.939692f, -1.000000f),
                new Vector3(0.000000f, 1.000000f, 1.000000f),
                new Vector3(-0.342020f, 0.939693f, 1.000000f),
                new Vector3(-0.642788f, 0.766044f, 1.000000f),
                new Vector3(-0.866025f, 0.500000f, 1.000000f),
                new Vector3(-0.984808f, 0.173648f, 1.000000f),
                new Vector3(-0.984808f, -0.173648f, 1.000000f),
                new Vector3(-0.866025f, -0.500000f, 1.000000f),
                new Vector3(-0.642788f, -0.766044f, 1.000000f),
                new Vector3(-0.342020f, -0.939692f, 1.000000f),
                new Vector3(-0.000000f, -1.000000f, 1.000000f),
                new Vector3(0.342020f, -0.939693f, 1.000000f),
                new Vector3(0.642787f, -0.766045f, 1.000000f),
                new Vector3(0.866025f, -0.500000f, 1.000000f),
                new Vector3(0.984808f, -0.173648f, 1.000000f),
                new Vector3(0.984808f, 0.173648f, 1.000000f),
                new Vector3(0.866026f, 0.500000f, 1.000000f),
                new Vector3(0.642788f, 0.766044f, 1.000000f),
                new Vector3(0.342021f, 0.939692f, 1.000000f)
            };

            int[] triangles =
            {
                14, 31, 13, 7, 24, 6, 15, 32, 14, 8, 25, 7, 16, 33, 15, 9, 26, 8,
                2, 19, 1, 17, 34, 16, 10, 27, 9, 3, 20, 2, 18, 35, 17, 11, 28, 10,
                4, 21, 3, 18, 19, 36, 12, 29, 11, 5, 22, 4, 12, 31, 30, 6, 23, 5,
                14, 32, 31, 7, 25, 24, 15, 33, 32, 8, 26, 25, 16, 34, 33, 9, 27, 26,
                2, 20, 19, 17, 35, 34, 10, 28, 27, 3, 21, 20, 18, 36, 35, 11, 29, 28,
                4, 22, 21, 18, 1, 19, 12, 30, 29, 5, 23, 22, 12, 13, 31, 6, 24, 23
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}