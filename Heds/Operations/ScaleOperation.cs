using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// An operation that scales the entire mesh bigger or smaller.
    /// </summary>
    public class ScaleOperation : IOperation
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
        
        public void Apply(Mesh mesh)
        {
            foreach (var vertex in mesh.Vertices)
            {
                var pos = vertex.Position;
                pos.Scale(_scale);
                vertex.Position = pos;
            }
        }
    }
}