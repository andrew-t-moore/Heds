using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// Builds a new sphere where all vertices have been clamped within two radii
    /// (based on the origin) - an inner radius and an outer radius.
    /// </summary>
    public class ClampVerticesWithinRadiiOperation : VertexTransformOperation
    {
        private readonly float _innerRadius;
        private readonly float _outerRadius;

        public ClampVerticesWithinRadiiOperation(float innerRadius, float outerRadius)
        {
            _innerRadius = innerRadius;
            _outerRadius = outerRadius;
        }
        
        public override Vector3 Transform(Vector3 vertexPosition)
        {
            var magnitude = vertexPosition.magnitude;
            var newMagnitude = Mathf.Clamp(magnitude, _innerRadius, _outerRadius);
            return vertexPosition.normalized * newMagnitude;
        }
    }
}