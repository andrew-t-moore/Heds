namespace Heds.Primitives
{
    public static class QuadCube
    {
        /// <summary>
        /// Creates a cube mesh where each face is a quad.
        /// </summary>
        public static Mesh Create()
        {
            return new Mesh()
                .AddVertex(0.5f, 0.5f, -0.5f, out var v0)
                .AddVertex(-0.5f, 0.5f, -0.5f, out var v1)
                .AddVertex(-0.5f, 0.5f, 0.5f, out var v2)
                .AddVertex(0.5f, 0.5f, 0.5f, out var v3)
                .AddVertex(0.5f, -0.5f, -0.5f, out var v4)
                .AddVertex(-0.5f, -0.5f, -0.5f, out var v5)
                .AddVertex(-0.5f, -0.5f, 0.5f, out var v6)
                .AddVertex(0.5f, -0.5f, 0.5f, out var v7)
                .AddFace(new[] { v0, v1, v2, v3 }, out _)
                .AddFace(new[] { v3, v2, v6, v7 }, out _)
                .AddFace(new[] { v4, v0, v3, v7 }, out _)
                .AddFace(new[] { v4, v5, v1, v0 }, out _)
                .AddFace(new[] { v1, v5, v6, v2 }, out _)
                .AddFace(new[] { v7, v6, v5, v4 }, out _);
        }
    }
}