namespace Heds.Operations
{
    /// <summary>
    /// Detaches any half-edges that don't have an incident face.
    /// </summary>
    public class DetachUnusedHalfEdgesOperation : IOperation
    {
        public void Apply(Mesh mesh)
        {
            for (var i = mesh.HalfEdges.Count - 1; i >= 0; i--)
            {
                var he = mesh.HalfEdges[i];
                if (he.Face == null)
                {
                    mesh.DetachHalfEdge(he);
                }
            }
        }
    }
}