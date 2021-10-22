namespace Heds.Primitives
{
    public static class Icosahedron
    {
        /// <summary>
        /// Creates an icosahedron.
        /// </summary>
        public static Mesh Create()
        {
            const float tao = 1.61803399f;

            // TODO: the resulting icosahedron might be a surprising size.
            // TODO: would be better if it was a unit sphere or something.
            return new Mesh()
                .AddVertex(1f, tao, 0, out var v0)
                .AddVertex(-1, tao, 0, out var v1)
                .AddVertex(1, -tao, 0, out var v2)
                .AddVertex(-1, -tao, 0, out var v3)
                .AddVertex(0, 1, tao, out var v4)
                .AddVertex(0, -1, tao, out var v5)
                .AddVertex(0, 1, -tao, out var v6)
                .AddVertex(0, -1, -tao, out var v7)
                .AddVertex(tao, 0, 1, out var v8)
                .AddVertex(-tao, 0, 1, out var v9)
                .AddVertex(tao, 0, -1, out var v10)
                .AddVertex(-tao, 0, -1, out var v11)
                .AddFace(new[] { v0, v1, v4 }, out _)
                .AddFace(new[] { v1, v9, v4 }, out _)
                .AddFace(new[] { v4, v9, v5 }, out _)
                .AddFace(new[] { v5, v9, v3 }, out _)
                .AddFace(new[] { v2, v3, v7 }, out _)
                .AddFace(new[] { v3, v2, v5 }, out _)
                .AddFace(new[] { v7, v10, v2 }, out _)
                .AddFace(new[] { v0, v8, v10 }, out _)
                .AddFace(new[] { v0, v4, v8 }, out _)
                .AddFace(new[] { v8, v2, v10 }, out _)
                .AddFace(new[] { v8, v4, v5 }, out _)
                .AddFace(new[] { v8, v5, v2 }, out _)
                .AddFace(new[] { v1, v0, v6 }, out _)
                .AddFace(new[] { v11, v1, v6 }, out _)
                .AddFace(new[] { v3, v9, v11 }, out _)
                .AddFace(new[] { v6, v10, v7 }, out _)
                .AddFace(new[] { v3, v11, v7 }, out _)
                .AddFace(new[] { v11, v6, v7 }, out _)
                .AddFace(new[] { v6, v0, v10 }, out _)
                .AddFace(new[] { v9, v1, v11 }, out _);
        }
    }
}