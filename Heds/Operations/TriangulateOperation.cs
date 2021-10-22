using System.Collections.Generic;
using System.Linq;

namespace Heds.Operations
{
    /// <summary>
    /// An operation that ensures that all of the faces of the mesh
    /// are triangles.
    /// </summary>
    public class TriangulateOperation : IOperation
    {
        public Mesh Apply(Mesh oldMesh)
        {
            var newMesh = new Mesh();
            var vertexMap = new Dictionary<Vertex, Vertex>();

            foreach (var oldVertex in oldMesh.Vertices)
            {
                vertexMap[oldVertex] = newMesh.AddVertex(oldVertex.Position);
            }

            foreach (var oldFace in oldMesh.Faces)
            {
                var vertices = oldFace.HalfEdges
                    .Select(e => vertexMap[e.From])
                    .ToArray();

                if (oldFace.HalfEdges.Count == 3)
                {
                    newMesh.AddFace(vertices, out _);
                }
                else
                {
                    var p1 = vertices[0];

                    for (var i = 2; i < vertices.Length; i++)
                    {
                        var p2 = vertices[i - 1];
                        var p3 = vertices[i];

                        newMesh.AddFace(new[]{ p1, p2, p3 }, out _);
                    }
                }
            }

            return newMesh;
        }
    }
}