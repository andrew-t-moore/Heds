using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// An operation that scales the entire mesh bigger or smaller.
    /// </summary>
    public class ScaleOperation : VertexTransformOperation
    {
        private readonly Vector3 _scale;
        
        public ScaleOperation(float scaleX, float scaleY, float scaleZ) : this(new Vector3(scaleX, scaleY, scaleZ))
        {
        }

        public ScaleOperation(float scale) : this (new Vector3(scale, scale, scale))
        {
        }

        public ScaleOperation(Vector3 scale)
        {
            _scale = scale;
        }
        
        public override Vector3 Transform(Vector3 vertexPosition)
        {
            return new Vector3(
                vertexPosition.x * _scale.x,
                vertexPosition.y * _scale.y,
                vertexPosition.z * _scale.z
            );
        }
    }
}