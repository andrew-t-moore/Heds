using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// An operation that translates (moves) the mesh around.
    /// </summary>
    public class TranslateOperation : VertexTransformOperation
    {
        private readonly Vector3 _translation;

        public TranslateOperation(Vector3 translation)
        {
            _translation = translation;
        }

        public override Vector3 Transform(Vector3 vertexPosition)
        {
            return vertexPosition + _translation;
        }
    }
}