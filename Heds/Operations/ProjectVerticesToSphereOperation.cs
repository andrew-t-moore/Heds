using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// An operation that moves all of the vertices of a mesh inwards or
    /// outwards so that all vertices are on the surface of a sphere.
    /// </summary>
    public class ProjectVerticesToSphereOperation : VertexTransformOperation
    {
        private readonly float _radius;

        public ProjectVerticesToSphereOperation(float radius)
        {
            _radius = radius;
        }
        
        public override Vector3 Transform(Vector3 vertexPosition)
        {
            return vertexPosition.normalized * _radius;
        }
    }
}