namespace Heds.Operations
{
    /// <summary>
    /// An operation that moves all of the vertices of a mesh inwards or
    /// outwards so that all vertices are on the surface of a sphere.
    /// </summary>
    public class ProjectVerticesToSphereOperation : IOperation
    {
        private readonly float _radius;

        public ProjectVerticesToSphereOperation(float radius)
        {
            _radius = radius;
        }
        
        public void Apply(Mesh mesh)
        {
            foreach (var vertex in mesh.Vertices)
            {
                vertex.Position = vertex.Position.normalized * _radius;
            }
        }
    }
}