using System.Linq;

namespace Heds.Operations
{
    /// <summary>
    /// Detaches any vertices that don't have an incident half-edge.
    /// </summary>
    public class DetachUnusedVerticesOperation : IOperation
    {
        public void Apply(Mesh mesh)
        {
            for (var i = mesh.Vertices.Count - 1; i >= 0; i--)
            {
                var v = mesh.Vertices[i];
                if (!v.IncomingHalfEdges.Any() && !v.OutgoingHalfEdges.Any())
                {
                    mesh.DetachVertex(v);
                }
            }
        }
    }
}