using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// Builds a new sphere where all vertices have been clamped within two radii
    /// (based on the origin) - an inner radius and an outer radius.
    /// </summary>
    public class ClampVerticesWithinRadiiOperation : IOperation
    {
        private readonly float _innerRadius;
        private readonly float _outerRadius;

        public ClampVerticesWithinRadiiOperation(float innerRadius, float outerRadius)
        {
            _innerRadius = innerRadius;
            _outerRadius = outerRadius;
        }
        
        public void Apply(Mesh mesh)
        {
            foreach (var vertex in mesh.Vertices)
            {
                var magnitude = vertex.Position.magnitude;
                var newMagnitude = Mathf.Clamp(magnitude, _innerRadius, _outerRadius);
                vertex.Position = vertex.Position.normalized * newMagnitude;
            }
        }
    }
}