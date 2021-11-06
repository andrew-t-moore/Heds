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
        public void Apply(Mesh mesh)
        {
            var faces = mesh.Faces.ToArray();
            
            foreach (var existingFace in faces)
            {
                if (!existingFace.IsTriangle)
                {
                    var vertices = existingFace
                        .Vertices
                        .ToArray();

                    mesh.DetachFace(existingFace);

                    var p1 = vertices[0];

                    for (var i = 2; i < vertices.Length; i++)
                    {
                        var p2 = vertices[i - 1];
                        var p3 = vertices[i];

                        mesh.AddFace(new[]{ p1, p2, p3 }, out _);
                    }
                }
            }
        }
    }
}