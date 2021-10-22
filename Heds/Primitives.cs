using UnityEngine;

namespace Heds.Primitives
{
    public static class Primitives
    {
        public static Mesh CreateCube()
        {
            return new Mesh()
                .AddVertex(0.5f, 0.5f, -0.5f, out var v1)
                .AddVertex(-0.5f, 0.5f, -0.5f, out var v2)
                .AddVertex(-0.5f, 0.5f, 0.5f, out var v3)
                .AddVertex(0.5f, 0.5f, 0.5f, out var v4)
                .AddVertex(0.5f, -0.5f, -0.5f, out var v5)
                .AddVertex(-0.5f, -0.5f, -0.5f, out var v6)
                .AddVertex(-0.5f, -0.5f, 0.5f, out var v7)
                .AddVertex(0.5f, -0.5f, 0.5f, out var v8)
                // Top edges
                // ReSharper disable InconsistentNaming
                .AddPairOfHalfEdges(v1, v2, out var v1v2, out var v2v1)
                .AddPairOfHalfEdges(v2, v3, out var v2v3, out var v3v2)
                .AddPairOfHalfEdges(v3, v4, out var v3v4, out var v4v3)
                .AddPairOfHalfEdges(v4, v1, out var v4v1, out var v1v4)
                // Sides
                .AddPairOfHalfEdges(v4, v8, out var v4v8, out var v8v4)
                .AddPairOfHalfEdges(v3, v7, out var v3v7, out var v7v3)
                .AddPairOfHalfEdges(v2, v6, out var v2v6, out var v6v2)
                .AddPairOfHalfEdges(v1, v5, out var v1v5, out var v5v1)
                // Bottom edges
                .AddPairOfHalfEdges(v8, v7, out var v8v7, out var v7v8)
                .AddPairOfHalfEdges(v7, v6, out var v7v6, out var v6v7)
                .AddPairOfHalfEdges(v6, v5, out var v6v5, out var v5v6)
                .AddPairOfHalfEdges(v5, v8, out var v5v8, out var v8v5)
                // Diagonals
                .AddPairOfHalfEdges(v3, v1, out var v3v1, out var v1v3)
                .AddPairOfHalfEdges(v6, v8, out var v6v8, out var v8v6)
                .AddPairOfHalfEdges(v4, v7, out var v4v7, out var v7v4)
                .AddPairOfHalfEdges(v5, v4, out var v5v4, out var v4v5)
                .AddPairOfHalfEdges(v5, v2, out var v5v2, out var v2v5)
                .AddPairOfHalfEdges(v2, v7, out var v2v7, out var v7v2)
                // ReSharper restore InconsistentNaming
            
                // Top faces
                .AddFace(new[] { v1v2, v2v3, v3v1 }, out _)
                .AddFace(new[] { v1v3, v3v4, v4v1 }, out _)

                // Side 1
                .AddFace(new[] { v4v3, v3v7, v7v4 }, out _)
                .AddFace(new[] { v4v7, v7v8, v8v4 }, out _)
            
                // Side 2
                .AddFace(new[] { v5v1, v1v4, v4v5 }, out _)
                .AddFace(new[] { v5v4, v4v8, v8v5 }, out _)
            
                // Side 3
                .AddFace(new[] { v5v6, v6v2, v2v5 }, out _)
                .AddFace(new[] { v5v2, v2v1, v1v5 }, out _)
            
                // Side 4
                .AddFace(new[] { v2v6, v6v7, v7v2 }, out _)
                .AddFace(new[] { v2v7, v7v3, v3v2 }, out _)
            
                // Bottom faces
                .AddFace(new[] { v8v7, v7v6, v6v8 }, out _)
                .AddFace(new[] { v8v6, v6v5, v5v8 }, out _);
        }
        
        public static Mesh CreateIcosahedron()
        {
            const float tao = 1.61803399f;
            var mesh = new Mesh();
            var vertices = mesh.AddVertices(
                new Vector3(1f, tao, 0),
                new Vector3(-1, tao, 0),
                new Vector3(1, -tao, 0),
                new Vector3(-1, -tao, 0),
                new Vector3(0, 1, tao),
                new Vector3(0, -1, tao),
                new Vector3(0, 1, -tao),
                new Vector3(0, -1, -tao),
                new Vector3(tao, 0, 1),
                new Vector3(-tao, 0, 1),
                new Vector3(tao, 0, -1),
                new Vector3(-tao, 0, -1)
            );

            return mesh
                .AddFace(new[] { vertices[0], vertices[1], vertices[4] }, out _)
                .AddFace(new[] { vertices[1], vertices[9], vertices[4] }, out _)
                .AddFace(new[] { vertices[4], vertices[9], vertices[5] }, out _)
                .AddFace(new[] { vertices[5], vertices[9], vertices[3] }, out _)
                .AddFace(new[] { vertices[2], vertices[3], vertices[7] }, out _)
                .AddFace(new[] { vertices[3], vertices[2], vertices[5] }, out _)
                .AddFace(new[] { vertices[7], vertices[10], vertices[2] }, out _)
                .AddFace(new[] { vertices[0], vertices[8], vertices[10] }, out _)
                .AddFace(new[] { vertices[0], vertices[4], vertices[8] }, out _)
                .AddFace(new[] { vertices[8], vertices[2], vertices[10] }, out _)
                .AddFace(new[] { vertices[8], vertices[4], vertices[5] }, out _)
                .AddFace(new[] { vertices[8], vertices[5], vertices[2] }, out _)
                .AddFace(new[] { vertices[1], vertices[0], vertices[6] }, out _)
                .AddFace(new[] { vertices[11], vertices[1], vertices[6] }, out _)
                .AddFace(new[] { vertices[3], vertices[9], vertices[11] }, out _)
                .AddFace(new[] { vertices[6], vertices[10], vertices[7] }, out _)
                .AddFace(new[] { vertices[3], vertices[11], vertices[7] }, out _)
                .AddFace(new[] { vertices[11], vertices[6], vertices[7] }, out _)
                .AddFace(new[] { vertices[6], vertices[0], vertices[10] }, out _)
                .AddFace(new[] { vertices[9], vertices[1], vertices[11] }, out _);
        }
    }
}